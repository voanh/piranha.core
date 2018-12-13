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
using Piranha.Extend.Fields;

namespace Piranha.SubSites.Models.Regions
{
    public class SiteInfo
    {
        [Field]
        public PageField StartPage { get; set; }
        [Field]
        public StringField Culture { get; set; }
    }
}
