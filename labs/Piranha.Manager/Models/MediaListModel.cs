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
using System.Collections.Generic;

namespace Piranha.Manager.Models
{
    public class MediaListModel
    {
        public class MediaListItem
        {
            public Guid Id { get; set; }
            public string Filename { get; set; }
            public string PublicUrl { get; set; }
        }

        public Guid? CurrentFolder { get; set; }
        public IEnumerable<MediaListItem> Items { get; set; } = new List<MediaListItem>();
    }
}