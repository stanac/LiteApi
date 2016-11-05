/// <reference path="jquery.d.ts" />

$(function () {
    $(document).ajaxSend(function (event, jqxhr, settings) {
        var request = {
            type: settings.type,
            url: settings.url
        };
        var json = JSON.stringify(request, null, 2);
        $('#preRequest').text(json);        
    });

    $(document).ajaxComplete(function (event, xhr, settings) {
        var json = xhr.responseText;
        try {
            json = JSON.stringify(xhr.responseJSON, null, 2);
        }
        catch (m) {
            json = xhr.responseText;
        }
        $('#preResponse').text(json);
    });

    function get(url) {
        $.ajax({
            url: url,
            type: 'GET'
        });
    }

    $('#public').click(function() {
        get('/api/service/public')
    });

    $('#check, #logout, [id^="login"]').each(function() {
        var el = $(this);
        el.click(function() {
            get('/api/account/' + el.attr('id'));
        });
    });

});