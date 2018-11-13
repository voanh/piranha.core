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
    [Route("manager/api")]
    public class MediaApiController : ControllerBase
    {
        private readonly MediaListService _service;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="service">The current media service</param>
        public MediaApiController(MediaListService service)
        {
            _service = service;
        }

        [Route("media/list/{folderId:Guid?}")]
        public ActionResult<Models.MediaListModel> Panel(Guid? folderId = null)
        {
            return _service.GetByFolderId(folderId);
        }
    }
}