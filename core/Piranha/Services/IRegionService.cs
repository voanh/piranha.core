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
using System.Collections.Generic;

namespace Piranha.Services
{
    public interface IRegionService
    {
        /// <summary>
        /// Creates a new region.
        /// </summary>
        /// <param name="typeId">The content type id</param>
        /// <param name="regionId">The region id</param>
        /// <returns>The new region value</returns>
        object CreateDynamicRegion(Models.ContentType contentType, string regionId);

        /// <summary>
        /// Creates a dynamic region.
        /// </summary>
        /// <typeparam name="TValue">The value type</typeparam>
        /// <param name="type">The content type</param>
        /// <param name="regionId">The region id</param>
        /// <returns>The region value</returns>
        T CreateRegion<T>(Models.ContentType contentType, string regionId);
    }
}
