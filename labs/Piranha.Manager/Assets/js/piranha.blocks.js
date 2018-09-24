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

                        // Recalc form indexes
                        self.recalcBlocks();
                    }
                });
            } else {
                // Recalc form indexes
                self.recalcBlocks();
            }
        });
    };

    self.recalcBlocks = function () {
        var items = $('.body-content .blocks > .block');

        for (var n = 0; n < items.length; n++) {
            var inputs = $(items.get(n)).find('input, textarea, select');

            inputs.attr('id', function (i, val) {
                if (val)
                    return val.replace(/Blocks_\d+__/, 'Blocks_' + n + '__');
                return val;
            });
            inputs.attr('name', function (i, val) {
                if (val)
                    return val.replace(/Blocks\[\d+\]/, 'Blocks[' + n + ']');
                return val;
            });

            var content = $(items.get(n)).find('[contenteditable=true]');
            content.attr('data-id', function (i, val) {
                if (val)
                    return val.replace(/Blocks_\d+__/, 'Blocks_' + n + '__');
                return val;
            });

            var media = $(items.get(n)).find('button');
            media.attr('data-mediaid', function (i, val) {
                if (val)
                    return val.replace(/Blocks_\d+__/, 'Blocks_' + n + '__');
                return val;
            });

            var subitems = $(items.get(n)).find('.block-group-body .sortable-item');

            for (var s = 0; s < subitems.length; s++) {
                var subInputs = $(subitems.get(s)).find('input, textarea, select');

                subInputs.attr('id', function (i, val) {
                    if (val)
                        return val.replace(/Blocks_\d+__Items_\d+__/, 'Blocks_' + n + '__Items_' + s + '__');
                    return val;
                });
                subInputs.attr('name', function (i, val) {
                    if (val)
                        return val.replace(/Blocks\[\d+\].Items\[\d+\]/, 'Blocks[' + n + '].Items[' + s + ']');
                    return val;
                });

                var subContent = $(subitems.get(s)).find('[contenteditable=true]');
                subContent.attr('data-id', function (i, val) {
                    if (val)
                        return val.replace(/Blocks_\d+__Items_\d+__/, 'Blocks_' + n + '__Items_' + s + '__');
                    return val;
                });

                var subContent = $(subitems.get(s)).find('button');
                subContent.attr('data-mediaid', function (i, val) {
                    if (val)
                        return val.replace(/Blocks_\d+__Items_\d+__/, 'Blocks_' + n + '__Items_' + s + '__');
                    return val;
                });                    
            }
        }
    };

    $(document).on('click', '.block-remove', function (e) {
        e.preventDefault();

        $(this).closest('.block').remove();

        // Recalc form indexes
        self.recalcBlocks();
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