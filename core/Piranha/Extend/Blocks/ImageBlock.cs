/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using Piranha.Extend.Fields;

namespace Piranha.Extend.Blocks
{
    /// <summary>
    /// Image block.
    /// </summary>
    [BlockType(Name = "Image", Category = "Media", Icon = "fas fa-image")]
    public class ImageBlock : Block
    {
        /// <summary>
        /// Gets/sets the image body.
        /// </summary>
        public ImageField Body { get; set; }
        
        /// <summary>
        /// Gets the list title if the block is used as
        /// an item in a block group.
        /// </summary>
        /// <returns>The title</returns>
        public override string GetTitle(IApi api)
        {
            if (Body != null && Body.HasValue)
            {
                return $"<img class=\"img-thumbnail\" src=\"{api.Media.EnsureVersion(Body.Id.Value, 30, 30).Replace("~", "")}\">{Body.Media.Filename}";
            }
            return "No Image Selected";
        }        
    }
}
