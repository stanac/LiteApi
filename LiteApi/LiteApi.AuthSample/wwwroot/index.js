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
        var text = 'status code: ' + xhr.status + '\n\n';
        text += json || "undefined";

        $('#preResponse').text(text);
    });

    function get(url) {
        $.ajax({
            url: url,
            type: 'GET'
        });
    }

    $('#public').click(function() {
        get('/api/service/Public')
    });

    $('#protected1').click(function () {
        get('/api/service/ProtectedWithoutRolesOrClaims')
    });
    
    $('#check, #logout, [id^="login"]').each(function() {
        var el = $(this);
        el.click(function() {
            get('/api/account/' + el.attr('id'));
        });
    });

    $('#protected2, #protected3, #protected4').each(function () {
        var el = $(this);
        el.click(function () {
            var url = '/api/service/' + el.text().replace(/\s/g, '');
            get(url);
        });
    });

    $('#post_protected').click(function () {
        $.ajax({
            url: '/api/service/ProtectedPostRequest',
            type: 'POST'
        });
    });
});