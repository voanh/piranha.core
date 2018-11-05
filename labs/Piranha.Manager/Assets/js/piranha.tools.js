//
// Copyright (c) 2018 Håkan Edling
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
// 
// http://github.com/piranhacms/piranha.core
// 

if (typeof(piranha)  == 'undefined')
    piranha = {};

piranha.tools = new function() {
    'use strict';

    var self = this;

    /**
     * Checks if the given dom element has any text content.
     * 
     * @param {*} elm The dom element
     */
    self.isEmpty = function (elm) {
        return $(elm).text().replace(/\s/g, '') == '' && $(elm).find('img').length == 0;
    };
}