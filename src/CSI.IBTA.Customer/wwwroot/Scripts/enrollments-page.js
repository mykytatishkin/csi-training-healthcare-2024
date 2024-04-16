function showEnrollments(employeeId, employerId, page) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/Enrollments?employeeId=' + employeeId + '&employerId=' + employerId + '&pageNumber=' + page;
    fetchRoute(route, onSuccess, null);
}