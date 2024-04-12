function showClaims(employeeId, employerId, pageNumber) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/Claims?' + new URLSearchParams({
        pageNumber: pageNumber,
        employeeId: employeeId,
        employerId: employerId,
    });
    fetchRoute(route, onSuccess, null);
}