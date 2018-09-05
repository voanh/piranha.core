if (typeof(piranha)  == 'undefined')
    piranha = {};

piranha.blocks = new function() {
    'use strict';

    var self = this;

    self.init = function () {
        // Create block type list
        var types = sortable('.block-types', {
            items: ':not(.unsortable)',
            acceptFrom: false,
            copy: true
        });

        // Create the main block list
        var blocks = sortable('.blocks', {
            handle: '.sortable-handle',
            items: ':not(.unsortable)',
            acceptFrom: '.blocks,.block-types'
        });

        // Add sortable events
        blocks[0].addEventListener('sortupdate', function(e) {
            var item = e.detail.item;

            if ($(item).hasClass('block-type')) {
                //
                // New block dropped in block list, create and
                // insert editor view.
                //
                $.ajax({
                    url: piranha.baseUrl + 'manager/block/create',
                    method: 'POST',
                    contentType: 'application/json',
                    dataType: 'html',
                    data: JSON.stringify({
                        Type: $(item).data('typename'),
                        Index: e.detail.destination.index
                    }),
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

                        // Update the sortable list
                        sortable('.blocks', {
                            handle: '.sortable-handle',
                            items: ':not(.unsortable)',
                            acceptFrom: '.blocks,.block-types'
                        });

                        // Unhide
                        $('.blocks .loading').removeClass('loading');
                    }
                });
            } else {
                //
                // Existing block changed position in the list.
                //
            }
        });
    };

    $(document).on('click', '.block-remove', function (e) {
        e.preventDefault();

        $(this).closest('.block').remove();
    });

    $(document).on('focus', '.block .empty', function () {
        $(this).removeClass('empty');
        $(this).addClass('check-empty');
    });

    $(document).on('blur','.block .check-empty', function () {
        if (piranha.tools.isEmpty(this)) {
            $(this).removeClass('check-empty');    
            $(this).addClass('empty');
        }
    });    
};