/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Piranha.Services;

namespace Piranha.Manager.Controllers
{
    [Area("Manager")]
    [Route("manager")]
    public class BlockController : Controller
    {
        /// <summary>
        /// Type doesn't really matter here as we're just working with blocks. But either way
        /// we need to instantiate the Generic service.
        /// </summary>
        private readonly IContentService<Data.Page, Data.PageField, Piranha.Models.PageBase> _contentService;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="factory">The current content service factory</param>
        public BlockController(IContentServiceFactory factory)
        {
            _contentService = factory.CreatePageService();
        }

        /// <summary>
        /// Creates a new block from the given parameters and
        /// returns a partial view for it.
        /// </summary>
        /// <param name="model">The model</param>
        [HttpPost]
        [Route("block/create")]
        public IActionResult CreateBlock([FromBody]Models.BlockCreateModel model)
        {
            var block = (Extend.Block)_contentService.CreateBlock(model.Type);

            if (block != null) 
            {
                ViewData.TemplateInfo.HtmlFieldPrefix = $"Blocks[{model.Index}]";

                return View("EditorTemplates/ContentEditBlock", new Models.ContentEditBlock() {
                    Id = block.Id,
                    CLRType = block.GetType().FullName,
                    IsGroup = typeof(Extend.BlockGroup).IsAssignableFrom(block.GetType()),
                    Value = block
                });
            }
            return new NotFoundResult();
        }
    }
}