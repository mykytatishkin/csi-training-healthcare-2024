function showEmployers() {
    function onSuccess(data) {
        document.getElementById('content').innerHTML = data;
    }

    document.getElementById("employerBtn").setAttribute("class", "left view-btn-active");
    document.getElementById("claimBtn").setAttribute("class", "right view-btn");

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
    fetchRoute(route, onSuccess, null);
}

function showClaims() {
    function onSuccess(data) {
        document.getElementById('content').innerHTML = data;
    }

    document.getElementById("employerBtn").setAttribute("class", "left view-btn");
    document.getElementById("claimBtn").setAttribute("class", "right view-btn-active");

    hideLogo();
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

    route = '/Claims?NumberFilter=' + formData.get('NumberFilter') + '&EmployerFilter=' + formData.get('EmployerFilter') + "&ClaimStatusFilter=" + formData.get('ClaimStatusFilter');
    fetchRoute(route, onSuccess, null);
}

function searchAllClaims() {
    function onSuccess(data) {
        document.getElementById('content').innerHTML = data;
    }

    route = '/Claims';
    fetchRoute(route, onSuccess, null);
}

function getClaimsPage(sort, page, numberFilter, employerFilter, claimStatus) {
    function onSuccess(data) {
        document.getElementById('content').innerHTML = data;
    }

    route = '/Claims?sortOrder=' + sort + '&pageNumber=' + page + '&currentNumberFilter=' + numberFilter + '&currentEmployerFilter=' + employerFilter + "&claimStatusFilter=" + claimStatus;
    fetchRoute(route, onSuccess, null);
}

function redirectToEmployersMenu(employerId) {
    function onSuccess(data) {
        document.getElementById('content').innerHTML = data;
        showEmployerDetails(employerId);
    }

    route = '/Employers';
    fetchRoute(route, onSuccess, null);
}