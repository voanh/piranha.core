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
using Piranha.Manager.Services;

namespace Piranha.Manager.Controllers
{
    [Area("Manager")]
    [Route("manager")]
    public class MediaController : Controller
    {
        private readonly MediaListService _service;

        public MediaController(MediaListService service)
        {
            _service = service;
        }

        [Route("media/panel/{folderId:Guid?}")]
        public IActionResult Panel(Guid? folderId = null)
        {
            return View(_service.GetByFolderId(folderId));
        }
    }
}