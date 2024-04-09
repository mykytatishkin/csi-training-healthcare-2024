function showHome() {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/HomePartialView';
    fetchRoute(route, onSuccess, null);
}