/*
 * Copyright (c) 2016-2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

namespace Piranha.Manager.Models
{
    /// <summary>
    /// A non collection region.
    /// </summary>
    public class ContentEditRegionItem : ContentEditRegion
    {
        /// <summary>
        /// Gets/sets the available fields.
        /// </summary>
        public ContentEditFieldSet FieldSet { get; set; }

        /// <summary>
        /// Adds a field set to the region.
        /// </summary>
        /// <param name="fieldSet">The field set</param>
        public override void Add(ContentEditFieldSet fieldSet) {
            FieldSet = fieldSet;
        }
    }    
}