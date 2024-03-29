function showEmployees(employerId) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/Employees?employerId=' + employerId;
    fetchRoute(route, onSuccess, null);
}
function showError(id, text) {
    document.getElementById(id).innerText = text;
}

function searchEmployees(employerId) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    var form = document.getElementById('employees-search-form');
    var formData = new FormData(form);

    route = '/Employees?' + new URLSearchParams({
        FirstnameFilter: formData.get('FirstnameFilter'),
        LastnameFilter: formData.get('LastnameFilter'),
        SsnFilter: formData.get('SsnFilter'),
        EmployerId: employerId
    });

    fetchRoute(route, onSuccess, null);
}

function getEmployeesPage(employerId, page, firstnameFilter, lastnameFilter, ssnFilter) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/Employees?' + new URLSearchParams({
        employerId: employerId,
        pageNumber: page,
        currentFirstnameFilter: firstnameFilter,
        currentLastnameFilter: lastnameFilter,
        currentSsnFilter: ssnFilter
    });

    fetchRoute(route, onSuccess, null);
}