/*
 * Copyright (c) 2017-2018 Håkan Edling
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
    public class ParamRepository : IParamRepository
    {
        private readonly IAsyncDocumentSession _session;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="session">The current document session</param>
        public ParamRepository(IAsyncDocumentSession session)
        {
            _session = session;
        }

        /// <summary>
        /// Gets all available models.
        /// </summary>
        /// <returns>The available models</returns>
        public async Task<IEnumerable<Param>> GetAll()
        {
            return await _session
                .Query<Param>()
                .OrderBy(p => p.Key)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the model with the specified id.
        /// </summary>
        /// <param name="id">The unique id</param>
        /// <returns>The model, or null if it doesn't exist</returns>
        public Task<Param> GetById(string id)
        {
            return _session
                .LoadAsync<Param>(id);
        }

        /// <summary>
        /// Gets the model with the given internal id.
        /// </summary>
        /// <param name="key">The unique key</param>
        /// <returns>The model</returns>
        public Task<Param> GetByKey(string key)
        {
            return _session
                .Query<Param>()
                .Where(p => p.Key == key)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds or updates the given model in the database
        /// depending on its state.
        /// </summary>
        /// <param name="model">The model</param>
        public async Task Save(Param model)
        {
            await _session.StoreAsync(model, model.Id);
            await _session.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the model with the specified id.
        /// </summary>
        /// <param name="id">The unique id</param>
        public async Task Delete(string id)
        {
            _session.Delete(id);
            await _session.SaveChangesAsync();
        }
    }
}
