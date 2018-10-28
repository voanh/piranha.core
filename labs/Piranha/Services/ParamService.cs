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
using Piranha.Repositories;

namespace Piranha.Services
{
    public class ParamService : IParamService
    {
        private readonly IParamRepository _repo;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="repo">The current repository</param>
        public ParamService(IParamRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Gets all available models.
        /// </summary>
        /// <returns>The available models</returns>
        public Task<IEnumerable<Param>> GetAll()
        {
            return _repo.GetAll();
        }

        /// <summary>
        /// Gets the model with the specified id.
        /// </summary>
        /// <param name="id">The unique id</param>
        /// <returns>The model, or null if it doesn't exist</returns>
        public Task<Param> GetById(string id)
        {
            return _repo.GetById(id);
        }

        /// <summary>
        /// Gets the model with the given internal id.
        /// </summary>
        /// <param name="key">The unique key</param>
        /// <returns>The model</returns>
        public Task<Param> GetByKey(string key)
        {
            return _repo.GetByKey(key);
        }

        /// <summary>
        /// Adds or updates the given model in the database
        /// depending on its state.
        /// </summary>
        /// <param name="model">The model</param>
        public async Task Save(Param model)
        {
            // Ensure that key isn't empty
            if (string.IsNullOrEmpty(model.Key))
                throw new InvalidOperationException("Key can't be empty.");

            // Ensure that key is unique
            var param = await _repo.GetByKey(model.Key);
            if (param != null && param.Id != model.Id)
                throw new InvalidOperationException($"Key {model.Key} already exists.");

            await _repo.Save(model);
        }

        /// <summary>
        /// Deletes the model with the specified id.
        /// </summary>
        /// <param name="id">The unique id</param>
        public Task Delete(string id)
        {
            return _repo.Delete(id);
        }
    }
}
