window.onload = function () {
    showEmployeeList(1);
}

function showEmployeeList(employerId) {
    function onSuccess(data) {
        document.getElementById('partial-screen').innerHTML = data;
    }

    route = '/Employees?employerId=' + employerId;
    fetchRoute(route, onSuccess, null);
}