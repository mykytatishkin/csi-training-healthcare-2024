function showHome(employeeId) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = `/HomePartialView/${employeeId}`;
    fetchRoute(route, onSuccess, null);
}