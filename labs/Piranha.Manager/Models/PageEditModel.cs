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

namespace Piranha.Manager.Models
{
    /// <summary>
    /// The page edit view model.
    /// </summary>
    public class PageEditModel : Piranha.Models.PageBase, IContentEditModel
    {        
        /// <summary>
        /// Gets/sets the page type.
        /// </summary>
        public Piranha.Models.PageType PageType { get; set; }

        /// <summary>
        /// Gets/sets the available blocks.
        /// </summary>
        public new IList<ContentEditBlock> Blocks { get; set; } = new List<ContentEditBlock>();

        /// <summary>
        /// Gets/sets the available regions.
        /// </summary>
        public IList<ContentEditRegion> Regions { get; set; } = new List<ContentEditRegion>();

        /// <summary>
        /// Gets/sets the page content type.
        /// </summary>
        public Runtime.AppContentType PageContentType { get; set; }

        public PinnedRegionInfo PinnedRegions { get; set; } = new PinnedRegionInfo();
    }

    public class PinnedRegionInfo
    {
        public IList<RegionInfo> BeforeTitle { get; set; } = new List<RegionInfo>();
        public IList<RegionInfo> BeforeBody { get; set; } = new List<RegionInfo>();
        public IList<RegionInfo> AfterBody { get; set; } = new List<RegionInfo>();
        public IList<RegionInfo> UnPinned { get; set; } = new List<RegionInfo>();
    }

    public class RegionInfo
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public object Body { get; set; }
    }
}
