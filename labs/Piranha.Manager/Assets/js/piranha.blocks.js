if (typeof(piranha)  == 'undefined')
    piranha = {};

piranha.blocks = new function() {
    'use strict';

    var self = this;

    self.init = function () {
        var types = sortable('.block-types', {
            items: ':not(.unsortable)',
            acceptFrom: false,
            copy: true
        });
        var blocks = sortable('.blocks', {
            handle: '.sortable-handle',
            items: ':not(.unsortable)',
            acceptFrom: '.blocks,.block-types'
        });
        blocks[0].addEventListener('sortupdate', function(e) {
            var item = e.detail.item;

            if ($(item).hasClass('block-type')) {
                $.ajax({
                    url: '/manager/page/block/' + $(item).data('typename') + '/' + e.detail.destination.index,
                    method: 'GET',
                    dataType: 'html',
                    success: function (res) {
                        // Remove the block-type container
                        $('.blocks .block-type').remove();

                        // Add the new block at the requested position
                        $(res).insertBefore($('.blocks .block').get(e.detail.destination.index));

                        // If the new region contains a html editor, make sure
                        // we initialize it.
                        var editors = $(res).find('.block-editor').each(function () {
                            addInlineEditor('#' + this.id);
                        });

                        // Unhide
                        $('.blocks .loading').removeClass('loading');
                    }
                });
                console.log('Dropped new block [' + $(item).data('typename') + '] at position [' + e.detail.destination.index + ']');
            } else {
                console.log('Dropped existing block');
            }
        });
    };

    $(document).on('click', '.block-remove', function (e) {
        e.preventDefault();

        $(this).closest('.block').remove();
    });
};