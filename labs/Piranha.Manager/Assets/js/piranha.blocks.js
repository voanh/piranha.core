if (typeof(piranha)  == 'undefined')
    piranha = {};

piranha.blocks = new function() {
    'use strict';

    var self = this;

    self.init = function () {
        var sortables = sortable('.blocks', {
            handle: '.sortable-handle',
            items: ':not(.unsortable)'
        });
        for (var n = 0; n < sortables.length; n++) {
            // TODO!!! 
            //
            // Setup events
        }
    };
};