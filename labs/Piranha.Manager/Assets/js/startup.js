$(function () {
    $('[data-toggle="popover"]').popover({
        trigger: 'hover'
    });
    $('[data-toggle="tooltip"]').tooltip();

    $('.panel-close').click(function (e) {
        e.preventDefault();
        $(this).closest('.panel').removeClass('active');
    });

    $('.btn-panel-settings').click(function (e) {
        e.preventDefault();
        $('.panel-settings').toggleClass('active');
    });

    $('.btn-panel-region').click(function (e) {
        e.preventDefault();
        console.log($($(this).data('id')));
        $($(this).data('id')).toggleClass('active');
    });

    $('.notification').each(function () {
        var note = $(this);

        setTimeout(function () {
            note.addClass('visible');

            setTimeout(function () {
                note.removeClass('visible');
            }, 2500);
        }, 200);
    });

    $('form').submit(function (e) {
        // Copy all contenteditable fields to their
        // form element.
        $('.editor-area').each(function () {
            $('#' + $(this).attr('data-id')).val($(this).html());
        });    

        // Move along and submit the form
        return true;
    });
});