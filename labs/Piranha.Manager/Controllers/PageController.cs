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

namespace Piranha.Manager.Controllers
{
    [Area("Manager")]
    public class PageController : Controller
    {
        private readonly Services.PageEditService _service;

        public PageController(Services.PageEditService service)
        {
            _service = service;
        }

        [Route("manager/pages")]
        public IActionResult Index()
        {
            return View(_service.GetById(new Guid("a47bc4f1-1722-4e09-b596-ab25d7657afb")));
        }
    }
}