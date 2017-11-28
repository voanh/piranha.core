﻿/*
 * Copyright (c) 2017 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha
 * 
 */

using Microsoft.EntityFrameworkCore;
using Piranha.Data;
using System.Data;
using System.Linq;

namespace Piranha.Repositories
{
    public class ParamRepository : BaseRepository<Param>, IParamRepository
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="db">The current db context</param>
        /// <param name="cache">The optional model cache</param>
        public ParamRepository(IDb db, ICache cache = null) 
            : base(db, cache) { }

        /// <summary>
        /// Gets the model with the given key.
        /// </summary>
        /// <param name="key">The unique key</param>
        /// <returns>The model</returns>
        public Param GetByKey(string key) {
            var id = cache != null ? cache.Get<string>($"ParamKey_{key}") : null;
            Param model = null;

            if (!string.IsNullOrEmpty(id)) {
                model = GetById(id);
            } else {
                model = db.Params
                    .AsNoTracking()
                    .FirstOrDefault(p => p.Key == key);

                if (cache != null && model != null)
                    AddToCache(model);
            }
            return model;
        }

        #region Protected methods
        /// <summary>
        /// Adds a new model to the database.
        /// </summary>
        /// <param name="model">The model</param>
        protected override void Add(Param model) {
            PrepareInsert(model);

            db.Params.Add(model);
        }

        /// <summary>
        /// Updates the given model in the database.
        /// </summary>
        /// <param name="model">The model</param>
        protected override void Update(Param model) {
            PrepareUpdate(model);

            var param = db.Params.FirstOrDefault(p => p.Id == model.Id);
            if (param != null) {
                App.Mapper.Map<Param, Param>(model, param);
            }
        }

        /// <summary>
        /// Adds the given model to cache.
        /// </summary>
        /// <param name="model">The model</param>
        protected override void AddToCache(Param model) {
            cache.Set(model.Id, model);
            cache.Set($"ParamKey_{model.Key}", model.Id);
        }
        #endregion
    }
}
