/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms
 * 
 */

using Piranha.AttributeBuilder;
using Piranha.Models;
using System.Collections.Generic;

namespace Piranha.SubSites.Models
{
    /// <summary>
    /// Page for creating a sub site.
    /// </summary>
    [PageType(Title = "Sub Site", UseBlocks = false)]
    public class SubSitePage : Page<SubSitePage>
    {
        [Region(Title = "Site Information")]
        public Regions.SiteInfo SiteInfo { get; set; }
    }
}
