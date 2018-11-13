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
using Piranha.Services;

namespace Piranha.Manager.Services
{
    /// <summary>
    /// The media list service.
    /// </summary>
    public class MediaListService
    {
        private readonly IApi _api;

        public MediaListService(IApi api)
        {
            _api = api;
        }

        public MediaListModel GetByFolderId(Guid? folderId)
        {
            return new MediaListModel()
            {
                CurrentFolder = folderId,
                Items = _api.Media.GetAll(folderId).Select(m => new MediaListModel.MediaListItem
                {
                    Id = m.Id,
                    Filename = m.Filename,
                    PublicUrl = m.PublicUrl.Replace("~", "")
                })
            };
        }
    }
}
 