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
    public interface IContentEditModel
    {
        /// <summary>
        /// Gets/sets the available regions.
        /// </summary>
        IList<ContentEditRegion> Regions { get; set; }
    }
}
