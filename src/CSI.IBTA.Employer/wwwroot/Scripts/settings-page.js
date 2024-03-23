function showSettings() {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/Settings';
    fetchRoute(route, onSuccess, null);
}