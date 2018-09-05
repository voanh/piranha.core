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