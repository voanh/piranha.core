/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha
 * 
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Piranha.Models;

namespace Piranha.Repositories
{
    public interface IAliasRepository : IBaseRepository<Alias>
    {
        /// <summary>
        /// Gets all available models.
        /// </summary>
        /// <param name="siteId">The optional site id</param>
        /// <returns>The available models</returns>
        Task<IEnumerable<Alias>> GetAll(string siteId = null);

        /// <summary>
        /// Gets the model with the given alias url.
        /// </summary>
        /// <param name="url">The unique url</param>
        /// <param name="siteId">The optional site id</param>
        /// <returns>The model</returns>
        Task<Alias> GetByAliasUrl(string url, string siteId = null);

        /// <summary>
        /// Gets the models with the given redirect url.
        /// </summary>
        /// <param name="url">The unique url</param>
        /// <param name="siteId">The optional site id</param>
        /// <returns>The models</returns>
        Task<IEnumerable<Alias>> GetByRedirectUrl(string url, string siteId = null);
    }
}
