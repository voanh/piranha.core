/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using System;

namespace Piranha.Models
{
    /// <summary>
    /// Information about a content revision.
    /// </summary>
    public class RevisionInfo
    {
        /// <summary>
        /// Gets/sets the unique id of the revision.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets/sets the id of the content this item
        /// is a revision of.
        /// </summary>
        public Guid ContentId { get; set; }

        /// <summary>
        /// Gets/sets the created date.
        /// </summary>
        public DateTime Created { get; set; }
    }
}
