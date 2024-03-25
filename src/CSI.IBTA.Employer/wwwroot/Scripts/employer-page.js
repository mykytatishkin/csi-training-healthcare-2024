function showEmployer() {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/Employer';
    fetchRoute(route, onSuccess, null);
}