const conn = new SignalR.HubConnectionBuilder().withUrl('/bitbarHub').build();

let bitbar = new Bitbar(
    'canvas', 'title', 'count',
    'my title',
    '#00ff00', '#00ff00',
    0, 1000,
    '#000000',
    null); 


conn.on('request_souce', () => {
    let source = {
        'type': 'bitbar',
        'source_id': getParameterByName('id', location.href)
    };

    conn.invoke('ProvideSource', JSON.stringify(source));
});


conn.on('update_source', (sourceJson) => {
    source = JSON.parse(sourceJson);

    bitbar = new Bitbar(
        'canvas', 'title', 'count',
        'my title',
        bitbar['fill_color'], bitbar['fill_color'],
        0, 1000,
        bitbar['target_color'],
        bitbar['image']); 
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
