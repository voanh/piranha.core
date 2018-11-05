/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

namespace Piranha.Manager.Models
{
    /// <summary>
    /// Information about a field that should be rendered in
    /// the UI, for example default rendering of block groups.
    /// </summary>
    public class FieldInfo
    {
        /// <summary>
        /// Gets/sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets/sets the property name of the field.
        /// </summary>
        public string PropertyName { get; set; }
    }
}