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
});