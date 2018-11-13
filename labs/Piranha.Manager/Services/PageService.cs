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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Piranha.Manager;
using Piranha.Manager.Models;
using Piranha.Models;
using Piranha.Services;

namespace Piranha.Manager.Models
{
    public class PageServiceModel
    {
        public PageInfo Page { get; set; }
        public PageType PageType { get; set; }
    }
}

namespace Piranha.Manager.Services
{
    /// <summary>
    /// The page edit service.
    /// </summary>
    public class PageService
    {
        private readonly IApi _api;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="api">The current api</param>
        public PageService(IApi api) 
        {
            _api = api;
        }

        public PageServiceModel GetById(Guid id)
        {
            var model = new PageServiceModel();

            model.Page = _api.Pages.GetById<PageInfo>(id);
            model.PageType = _api.PageTypes.GetById(model.Page.TypeId);

            return model;
        }
    }
}
