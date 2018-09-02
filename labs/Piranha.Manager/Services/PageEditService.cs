/*
 * Copyright (c) 2016-2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Piranha.Manager;
using Piranha.Manager.Models;
using Piranha.Services;

namespace Piranha.Manager.Services
{
    /// <summary>
    /// The page edit service.
    /// </summary>
    public class PageEditService : ContentEditService
    {
        private readonly IApi _api;
        private readonly IContentService<Data.Page, Data.PageField, Piranha.Models.PageBase> _service;

        /// <summary>
        /// Gets the region service.
        /// </summary>
        protected override IRegionService RegionService => _service;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="api">The current api</param>
        /// <param name="factory">The content service factory</param>
        public PageEditService(IApi api, IContentServiceFactory factory) 
        {
            _api = api;
            _service = factory.CreatePageService();
        }

        /// <summary>
        /// Saves the page model.
        /// </summary>
        /// <param name="model">The page edit model</param>
        /// <param name="alias">The suggested alias</param>
        /// <param name="publish">If the page should be published</param>
        /// <returns>If the page was successfully saved</returns>
        public bool Save(PageEditModel model, out string alias, bool? publish = null) 
        {
            var page = _api.Pages.GetById(model.Id);
            alias = null;

            if (page == null) 
            {
                page = Piranha.Models.DynamicPage.Create(_api, model.TypeId);
            } 
            else 
            {
                if (model.Slug != page.Slug && model.Published.HasValue)
                    alias = page.Slug;
            }

            var type = _api.PageTypes.GetById(page.TypeId);

            Module.Mapper.Map<PageEditModel, Piranha.Models.PageBase>(model, page);

            SaveRegions(model, page, type);
            SaveBlocks(model, page);

            if (publish.HasValue) 
            {
                if (publish.Value && !page.Published.HasValue)
                    page.Published = DateTime.Now;
                else if (!publish.Value)
                    page.Published = null;
            }

            _api.Pages.Save(page);
            model.Id = page.Id;

            return true;
        }

        /// <summary>
        /// Gets the edit model for the page with the given id.
        /// </summary>
        /// <param name="id">The page id</param>
        /// <returns>The page model</returns>
        public PageEditModel GetById(Guid id) 
        {
            var page = _api.Pages.GetById(id);

            if (page != null) 
            {
                var clrPage = _api.Pages.GetById<Piranha.Models.PageBase>(id);

                var model = Module.Mapper.Map<Piranha.Models.PageBase, PageEditModel>(page);
                model.PageType = _api.PageTypes.GetById(model.TypeId);
                model.PageContentType = App.ContentTypes.GetById(model.PageType.ContentTypeId);

                LoadRegions(page, model, model.PageType);
                LoadBlocks(page, model);

                foreach (var region in model.PageType.Regions.Where(r => r.Position == Piranha.Models.RegionTypePosition.BeforeTitle))
                {
                    model.PinnedRegions.BeforeTitle.Add(new RegionInfo 
                    { 
                        Id = region.Id, 
                        Title = !string.IsNullOrEmpty(region.Title) ? region.Title : region.Id,
                        Body = clrPage.GetType().GetProperty(region.Id, App.PropertyBindings).GetValue(clrPage)
                    });
                }
                foreach (var region in model.PageType.Regions.Where(r => r.Position == Piranha.Models.RegionTypePosition.BeforeBody))
                {
                    model.PinnedRegions.BeforeBody.Add(new RegionInfo 
                    { 
                        Id = region.Id, 
                        Title = !string.IsNullOrEmpty(region.Title) ? region.Title : region.Id,
                        Body = clrPage.GetType().GetProperty(region.Id, App.PropertyBindings).GetValue(clrPage)
                    });
                }
                foreach (var region in model.PageType.Regions.Where(r => r.Position == Piranha.Models.RegionTypePosition.AfterBody))
                {
                    model.PinnedRegions.AfterBody.Add(new RegionInfo 
                    { 
                        Id = region.Id,
                        Title = !string.IsNullOrEmpty(region.Title) ? region.Title : region.Id,
                        Body = clrPage.GetType().GetProperty(region.Id, App.PropertyBindings).GetValue(clrPage)
                    });
                }
                foreach (var region in model.PageType.Regions.Where(r => r.Position == Piranha.Models.RegionTypePosition.NotSpecified))
                {
                    model.PinnedRegions.UnPinned.Add(new RegionInfo 
                    { 
                        Id = region.Id, 
                        Title = !string.IsNullOrEmpty(region.Title) ? region.Title : region.Id,
                        Body = clrPage.GetType().GetProperty(region.Id, App.PropertyBindings).GetValue(clrPage)
                    });
                }

                return model;
            }
            throw new KeyNotFoundException($"No page found with the id '{id}'");
        }

        /// <summary>
        /// Refreshes the model after an unsuccessful save.
        /// </summary>
        public PageEditModel Refresh(PageEditModel model) 
        {
            if (!string.IsNullOrWhiteSpace(model.TypeId)) 
            {
                model.PageType = _api.PageTypes.GetById(model.TypeId);
                model.PageContentType = App.ContentTypes.GetById(model.PageType.ContentTypeId);
            }
            return model;
        }

        /// <summary>
        /// Creates a new edit model with the given page typeparamref.
        /// </summary>
        /// <param name="pageTypeId">The page type id</param>
        /// <param name="siteId">The optional site id</param>
        /// <returns>The page model</returns>        
        public PageEditModel Create(string pageTypeId, Guid? siteId = null) 
        {
            var type = _api.PageTypes.GetById(pageTypeId);

            if (!siteId.HasValue) 
            {
                var site = _api.Sites.GetDefault();

                if (site != null)
                    siteId = site.Id;
            }

            if (type != null) 
            {
                var page = Piranha.Models.DynamicPage.Create(_api, pageTypeId);
                var model = Module.Mapper.Map<Piranha.Models.PageBase, PageEditModel>(page);

                model.SiteId = siteId.Value;
                model.PageType = type;
                model.PageContentType = App.ContentTypes.GetById(type.ContentTypeId);
                model.ContentType = model.PageContentType != null ? model.PageContentType.Id : null;

                LoadRegions(page, model, type);

                return model;
            }
            throw new KeyNotFoundException($"No page type found with the id '{pageTypeId}'");
        }

        /// <summary>
        /// Loads the available blocks from the source model into the destination.
        /// </summary>
        /// <param name="src">The source</param>
        /// <param name="dest">The destination</param>
        private void LoadBlocks(Piranha.Models.DynamicPage src, PageEditModel dest) 
        {
            foreach (var srcBlock in src.Blocks) 
            {
                var block = new ContentEditBlock
                {
                    Id = srcBlock.Id,
                    CLRType = srcBlock.GetType().FullName,
                    IsGroup = typeof(Extend.BlockGroup).IsAssignableFrom(srcBlock.GetType()),
                    Value = srcBlock
                };

                if (typeof(Extend.BlockGroup).IsAssignableFrom(srcBlock.GetType())) 
                {
                    foreach (var subBlock in ((Extend.BlockGroup)srcBlock).Items) 
                    {
                        block.Items.Add(new ContentEditBlock 
                        {
                            Id = subBlock.Id,
                            CLRType = subBlock.GetType().FullName,
                            Value = subBlock
                        });
                    }
                }
                dest.Blocks.Add(block);
            }
        }

        private void SaveBlocks(PageEditModel src, Piranha.Models.DynamicPage dest) 
        {
            // Clear the block list
            dest.Blocks.Clear();

            // And rebuild it
            foreach (var srcBlock in src.Blocks) 
            {
                dest.Blocks.Add(srcBlock.Value);

                if (typeof(Extend.BlockGroup).IsAssignableFrom(srcBlock.Value.GetType())) 
                {
                    foreach (var subBlock in srcBlock.Items)
                        ((Extend.BlockGroup)srcBlock.Value).Items.Add(subBlock.Value);
                }
            }
        }
    }
}
