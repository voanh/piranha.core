$(function () {
    $('[data-toggle="popover"]').popover({
        trigger: 'hover'
    });

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
});