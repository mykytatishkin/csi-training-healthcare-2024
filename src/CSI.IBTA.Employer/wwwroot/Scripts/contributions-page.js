function showContributions() {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/Contributions';
    fetchRoute(route, onSuccess, null);
}