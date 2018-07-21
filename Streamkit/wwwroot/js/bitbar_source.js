$(document).ready(function () {
    var conn = new signalR.HubConnectionBuilder().withUrl('/bitbarHub').build();

    var bitbar = new Bitbar(
        'canvas', 'title', 'count',
        '',
        '#00ff00', '#00ff00',
        0, 1000,
        '#000000',
        null);


    conn.on('request_source', () => {
        let source = {
            'type': 'bitbar',
            'source_id': getParameterByName('id', location.href)
        };

        conn.invoke('ReceiveSource', JSON.stringify(source)).catch(err => console.error(err.toString()));;
    });


    conn.on('update_source', (sourceJson) => {
        source = JSON.parse(sourceJson);

        bitbar = new Bitbar(
            'canvas', 'title', 'count',
            '',
            source['fill_color'], source['target_color'],
            source['value'], source['max_value'],
            source['target_color'],
            source['image']);
        bitbar.updateCurrentBitCount(source['value']);
    });

    conn.start().catch(err => console.error(err.toString()));
});


function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
