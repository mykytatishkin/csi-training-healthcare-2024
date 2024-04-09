function showEnrollments() {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/Enrollments';
    fetchRoute(route, onSuccess, null);
}