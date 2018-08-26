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
    /// A collection region.
    /// </summary>
    public class ContentEditRegionCollection : ContentEditRegion
    {
        /// <summary>
        /// Gets/sets the available fieldsets.
        /// </summary>
        public IList<ContentEditFieldSet> FieldSets { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ContentEditRegionCollection() {
            FieldSets = new List<ContentEditFieldSet>();
        }

        /// <summary>
        /// Adds a field set to the region.
        /// </summary>
        /// <param name="fieldSet">The field set</param>
        public override void Add(ContentEditFieldSet fieldSet) {
            FieldSets.Add(fieldSet);
        }
    }
}