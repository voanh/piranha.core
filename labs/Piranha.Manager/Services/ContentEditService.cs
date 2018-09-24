/*
 * Copyright (c) 2016-2017 Håkan Edling
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
using Piranha.Models;
using Piranha.Services;

namespace Piranha.Manager.Services
{
    /// <summary>
    /// The page edit service.
    /// </summary>
    public abstract class ContentEditService
    {
        /// <summary>
        /// Gets the region service.
        /// </summary>
        protected abstract IRegionService RegionService { get; }

        /// <summary>
        /// Creates a new edit region model for the given region type and value.
        /// </summary>
        /// <param name="region">The region type</param>
        /// <param name="value">The region value</param>
        /// <returns>The edit model</returns>
        public ContentEditRegion CreateRegion(Piranha.Models.RegionType region, object value) {
            ContentEditRegion editRegion;

            if (region.Collection) {
                editRegion = new ContentEditRegionCollection();
            } else {
                editRegion = new ContentEditRegionItem();
            }
            editRegion.Id = region.Id;
            editRegion.Title = region.Title ?? region.Id;
            editRegion.CLRType = editRegion.GetType().FullName;
            editRegion.Description = region.Description;

            IList items = new List<object>();

            if (region.Collection)
                items = (IList)value;
            else items.Add(value);

            foreach (var item in items) {
                if (region.Fields.Count == 1) {
                    var itemTitle = "";

                    // Get the item title if this is a collection region.
                    if (region.Collection) {
                        if (item != null)
                            itemTitle = ((Extend.IField)item).GetTitle();
                        if (string.IsNullOrWhiteSpace(itemTitle) && !string.IsNullOrWhiteSpace(region.ListTitlePlaceholder))
                            itemTitle = region.ListTitlePlaceholder;
                        else itemTitle = "Item";
                    }

                    var set = new ContentEditFieldSet() {
                        new ContentEditField() {
                            Id = region.Fields[0].Id,
                            Title = region.Fields[0].Title ?? region.Fields[0].Id,
                            CLRType = item.GetType().FullName,
                            Options = region.Fields[0].Options,
                            Placeholder = region.Fields[0].Placeholder,
                            Value = (Extend.IField)item
                        }
                    };
                    set.ListTitle = itemTitle;
                    set.NoExpand = !region.ListExpand;

                    editRegion.Add(set);
                } else {
                    var fieldData = (IDictionary<string, object>)item;
                    var fieldSet = new ContentEditFieldSet();

                    foreach (var field in region.Fields) {
                        if (fieldData.ContainsKey(field.Id)) {
                            // Get the item title if this is a collection region.
                            if (region.Collection) {
                                if (!string.IsNullOrWhiteSpace(region.ListTitleField) && field.Id == region.ListTitleField) {
                                    var itemTitle = "";

                                    if (fieldData[field.Id] != null)
                                        itemTitle = ((Extend.IField)fieldData[field.Id]).GetTitle();
                                    if (string.IsNullOrWhiteSpace(itemTitle) && !string.IsNullOrWhiteSpace(region.ListTitlePlaceholder))
                                        itemTitle = region.ListTitlePlaceholder;
                                    else if (string.IsNullOrWhiteSpace(itemTitle)) 
                                        itemTitle = "Item";

                                    fieldSet.ListTitle = itemTitle;
                                    fieldSet.NoExpand = !region.ListExpand;
                                }
                            }

                            fieldSet.Add(new ContentEditField() {
                                Id = field.Id,
                                Title = field.Title ?? field.Id,
                                CLRType = fieldData[field.Id].GetType().FullName,
                                Options = field.Options,
                                Placeholder = field.Placeholder,
                                Value = (Extend.IField)fieldData[field.Id]
                            });
                        }
                    }
                    editRegion.Add(fieldSet);
                }
            }
            return editRegion;
        }

        /// <summary>
        /// Loads all of the regions from the source model into the destination.
        /// </summary>
        /// <param name="src">The source</param>
        /// <param name="dest">The destination</param>
        protected void LoadRegions(IDynamicModel src, IContentEditModel dest, ContentType type) {
            if (type != null) {
                foreach (var region in type.Regions) {
                    var regions = (IDictionary<string, object>)src.Regions;

                    if (regions.ContainsKey(region.Id)) {
                        var editRegion = CreateRegion(region, regions[region.Id]);
                        dest.Regions.Add(editRegion);
                    }
                }
            }
        }

        /// <summary>
        /// Saves all of the regions from the source model into the destination.
        /// </summary>
        /// <param name="api">The current api</param>
        /// <param name="src">The source</param>
        /// <param name="dest">The destination</param>
        protected void SaveRegions(IContentEditModel src, Piranha.Models.IDynamicModel dest, ContentType type) {
            var modelRegions = (IDictionary<string, object>)dest.Regions;

            foreach (var region in src.Regions) {
                if (region is ContentEditRegionItem) {
                    if (!modelRegions.ContainsKey(region.Id))
                        modelRegions[region.Id] = RegionService.CreateDynamicRegion(type, region.Id);

                    var reg = (ContentEditRegionItem)region;

                    if (reg.FieldSet.Count == 1) {
                        modelRegions[region.Id] = reg.FieldSet[0].Value;
                    } else {
                        var modelFields = (IDictionary<string, object>)modelRegions[region.Id];

                        foreach (var field in reg.FieldSet) {
                            modelFields[field.Id] = field.Value;
                        }
                    }
                } else {
                    if (modelRegions.ContainsKey(region.Id)) {
                        var list = (Piranha.Models.IRegionList)modelRegions[region.Id];
                        var reg = (ContentEditRegionCollection)region;

                        // At this point we clear the values and rebuild them
                        list.Clear();

                        foreach (var set in reg.FieldSets) {
                            if (set.Count == 1) {
                                list.Add(set[0].Value);
                            } else {
                                var modelFields = (IDictionary<string, object>)RegionService.CreateDynamicRegion(type, region.Id);

                                foreach (var field in set) {
                                    modelFields[field.Id] = field.Value;
                                }
                                list.Add(modelFields);
                            }
                        }
                    }
                }
            }
        }
    }
}
