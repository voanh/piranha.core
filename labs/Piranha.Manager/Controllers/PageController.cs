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
    [Route("manager")]
    public class PageController : Controller
    {
        private readonly Services.PageEditService _service;
        private readonly Services.PageService _newService;

        /// <summary>
        /// Default controller.
        /// </summary>
        /// <param name="service">The main page edit service</param>
        public PageController(Services.PageEditService service, Services.PageService newService)
        {
            _service = service;
            _newService = newService;
        }

        /// <summary>
        /// Gets the sitemap view for the pages.
        /// </summary>
        [Route("pages")]
        public IActionResult Index()
        {
            return RedirectToAction("Edit", new { id = new Guid("a47bc4f1-1722-4e09-b596-ab25d7657afb") });
        }

        /// <summary>
        /// Adds a new page of the specified type.
        /// </summary>
        /// <param name="type">The page type</param>
        [Route("page/add/{type}")]
        public IActionResult Add(string type)
        {
            var model = _service.Create(type);

            if (model != null)
                return View("Edit", model);
            return NotFound();
        }

        /// <summary>
        /// Edits the page with the given id.
        /// </summary>
        /// <param name="id">The page id</param>
        [Route("page/{id:Guid}")]
        public IActionResult Edit(Guid id)
        {
            var model = _service.GetById(id);

            if (model != null)
                return View(model);
            return NotFound();
        }

        /// <summary>
        /// Edits the page with the given id.
        /// </summary>
        /// <param name="id">The page id</param>
        [Route("page/new/{id:Guid}")]
        public IActionResult EditNew(Guid id)
        {
            var model = _newService.GetById(id);

            if (model != null)
                return View(model);
            return NotFound();

        }

        /// <summary>
        /// Saves the given page model.
        /// </summary>
        /// <param name="model">The page model</param>
        [Route("page/save")]
        [HttpPost]
        public IActionResult Save(Models.PageEditModel model)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(model.Title)) {
                return BadRequest();
            }

            var ret = _service.Save(model, out var alias);

            // Save
            if (ret) {
                return RedirectToAction("Edit", new { id = model.Id });
            } else {
                return StatusCode(500);
            }
        }
    }
}