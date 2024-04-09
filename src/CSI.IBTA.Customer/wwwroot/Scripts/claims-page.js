function showClaims() {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/Claims';
    fetchRoute(route, onSuccess, null);
}