var conn;
var bitbar;
var bitbarImage;
var loaded = false;

$(document).ready(function () {
    bitbar = new Bitbar(
        'canvas', 'title', 'count',
        '',
        '#00ff00', '#00ff00',
        0, 1000,
        '#000000',
        null);;


    setInterval(connectionLoop, 2000);
});


// I'm to tired to deal with microsoft removing features for no good reason.
// Only the first update event works, so let's turn this into continous polling until we figure it out.
function connectionLoop() {
    if (!loaded) {
        let reqstr = "../API/Streamkit/BitbarSource?id=" + getParameterByName('id', location.href);

        $.getJSON(reqstr, function (source) {
            bitbar = new Bitbar(
                'canvas', 'title', 'count',
                '',
                source['fill_color'], source['target_color'],
                source['value'], source['max_value'],
                source['target_color'],
                source['image']);

            loaded = true;          
        }).fail(function (jqxhr, textStatus, error) {
            var err = textStatus + ", " + error;
            console.log("Request Failed: " + err);
        });
    }

    let reqstr = "../API/Streamkit/Bitbar?id=" + getParameterByName('id', location.href);
    $.getJSON(reqstr, function (source) {
        bitbar.updateCurrentBitCount(source['value']);
        bitbar.updateMaxBitCount(source['max_value']);
        bitbar.updateColor(source['fill_color']);
        bitbar.updateFillColor(source['target_color']);
        bitbar.updateFillAreaColor(source['target_color']);
    }).fail(function (jqxhr, textStatus, error) {
        var err = textStatus + ", " + error;
        console.log("Request Failed: " + err);
    });
}


function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
