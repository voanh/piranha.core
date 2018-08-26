/*
 * Copyright (c) 2016-2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using System.Collections.Generic;

namespace Piranha.Manager.Models
{
    /// <summary>
    /// A collection of page fields.
    /// </summary>
    public class ContentEditFieldSet : List<ContentEditField> 
    { 
        /// <summary>
        /// Gets/sets the possible list title.
        /// </summary>
        public string ListTitle { get; set; }

        /// <summary>
        /// Gets/sets if the field set shouldn't be
        /// expandable in lists.
        /// </summary>
        public bool NoExpand { get; set; }
    }    
}