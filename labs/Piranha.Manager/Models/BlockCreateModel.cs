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

namespace Piranha.Manager.Models
{
    /// <summary>
    /// Post model for creating a new block.
    /// </summary>
    public class BlockCreateModel
    {
        /// <summary>
        /// Gets/sets the requested block type.
        /// </summary>
        public string Type { get; set; } 

        /// <summary>
        /// Gets/sets the requested block index.
        /// </summary>
        public int Index { get; set; }
    }    
}