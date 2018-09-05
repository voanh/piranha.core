/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Piranha.Services;

namespace Piranha.Manager.Controllers
{
    [Area("Manager")]
    public class PageController : Controller
    {
        private readonly Services.PageEditService _service;
        private readonly IContentService<Data.Page, Data.PageField, Piranha.Models.PageBase> _contentService;

        public PageController(Services.PageEditService service, IContentServiceFactory factory)
        {
            _service = service;
            _contentService = factory.CreatePageService();
        }

        [Route("manager/pages")]
        public IActionResult Index()
        {
            return RedirectToAction("Edit", new { id = new Guid("a47bc4f1-1722-4e09-b596-ab25d7657afb") });
        }

        [Route("manager/page/add/{type}")]
        public IActionResult Add(string type)
        {
            var model = _service.Create(type);

            if (model != null)
                return View("Edit", model);
            return NotFound();
        }

        [Route("manager/page/{id:Guid}")]
        public IActionResult Edit(Guid id)
        {
            var model = _service.GetById(id);

            if (model != null)
                return View(model);
            return NotFound();
        }
    }
}