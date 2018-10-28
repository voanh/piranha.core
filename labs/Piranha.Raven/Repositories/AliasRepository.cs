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
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Piranha.Models;
using Piranha.Repositories;

namespace Piranha.Raven.Repositories
{
    public class AliasRepository : IAliasRepository
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="session">The current document session</param>
        public AliasRepository(IAsyncDocumentSession session)
        {
            _session = session;
        }

        /// <summary>
        /// Gets all available models.
        /// </summary>
        /// <param name="siteId">The optional site id</param>
        /// <returns>The available models</returns>
        public async Task<IEnumerable<Alias>> GetAll(string siteId = null)
        {
            var query = _session
                .Query<Alias>();
            
            if (!string.IsNullOrEmpty(siteId))
                query = query.Where(a => a.SiteId == siteId);

            return await query
                .OrderBy(a => a.AliasUrl)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the model with the specified id.
        /// </summary>
        /// <param name="id">The unique id</param>
        /// <returns>The model, or null if it doesn't exist</returns>
        public Task<T> GetById(string id)
        {
            return _session
                .LoadAsync<Alias>(id);
        }

        /// <summary>
        /// Gets the model with the given alias url.
        /// </summary>
        /// <param name="url">The unique url</param>
        /// <param name="siteId">The optional site id</param>
        /// <returns>The model</returns>
        public Task<Alias> GetByAliasUrl(string url, string siteId = null)
        {
            var query = _session
                .Query<Alias>()
                .Where(a => a.AliasUrl == url);

            if (!string.IsNullOrEmpty(siteId))
                query = query.Where(a => a.SiteId == siteId);

            return query
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the models with the given redirect url.
        /// </summary>
        /// <param name="url">The unique url</param>
        /// <param name="siteId">The optional site id</param>
        /// <returns>The models</returns>
        public async Task<IEnumerable<Alias>> GetByRedirectUrl(string url, string siteId = null)
        {
            var query = _session
                .Query<Alias>()
                .Where(a => a.RedirectUrl == url);

            if (!string.IsNullOrEmpty(siteId))
                query = query.Where(a => a.SiteId == siteId);

            return await query
                .ToListAsync();

        }

        /// <summary>
        /// Adds or updates the given model in the database
        /// depending on its state.
        /// </summary>
        /// <param name="model">The model</param>
        public Task Save(Alias model)
        {
            await _session.StoreAsync(model, model.Id);
            await _session.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the model with the specified id.
        /// </summary>
        /// <param name="id">The unique id</param>
        public Task Delete(string id)
        {
            _session.Delete(id);
            await _session.SaveChangesAsync();
        }
    }
}
