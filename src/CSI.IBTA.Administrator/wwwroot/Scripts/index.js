function showEmployers() {
    function onSuccess(data) {
        document.getElementById('content').innerHTML = data;
    }

    route = '/Employers';
    fetchRoute(route, onSuccess, null);
}

function searchEmployers() {
    function onSuccess(data) {
        document.getElementById('content').innerHTML = data;
    }

    var form = document.getElementById('employer-search-form');

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);

    route = '/Employers?NameFilter=' + formData.get('NameFilter') + '&CodeFilter=' + formData.get('CodeFilter');
    fetchRoute(route, onSuccess, null);
}

function searchAllEmployers() {
    function onSuccess(data) {
        document.getElementById('content').innerHTML = data;
    }

    route = '/Employers';
    fetchRoute(route, onSuccess, null);
}

function getEmployersPage(sort, page, filter) {
    function onSuccess(data) {
        document.getElementById('content').innerHTML = data;
    }

    route = '/Employers?sortOrder=' + sort + '&pageNumber=' + page + '&currentFilter=' + filter;
    console.log(route);
    fetchRoute(route, onSuccess, null);
}

function showClaims() {
    function onSuccess(data) {
        document.getElementById('content').innerHTML = data;
    }

    route = '/Claims';
    fetchRoute(route, onSuccess, null);
}

function searchClaims() {
    function onSuccess(data) {
        document.getElementById('content').innerHTML = data;
    }

    var form = document.getElementById('claims-search-form');

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);

    route = '/Claims?NumberFilter=' + formData.get('NumberFilter') + '&EmployerFilter=' + formData.get('EmployerFilter');
    fetchRoute(route, onSuccess, null);
}

function searchAllClaims() {
    function onSuccess(data) {
        document.getElementById('content').innerHTML = data;
    }

    route = '/Claims';
    fetchRoute(route, onSuccess, null);
}

function getClaimsPage(sort, page, filter) {
    function onSuccess(data) {
        document.getElementById('content').innerHTML = data;
    }

    route = '/Claims?sortOrder=' + sort + '&pageNumber=' + page + '&currentFilter=' + filter;
    console.log(route);
    fetchRoute(route, onSuccess, null);
}