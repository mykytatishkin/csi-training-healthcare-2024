
function showEmployerPackagePlans(employerId) {
    function onSuccess(data) {
        document.getElementById('employer-partial-action').innerHTML = data;
    }

    function onFailure(statusCode) {
        console.error('There was a problem with the fetch operation:', statusCode);
        showError("employer-administration-errors", "Server error");
    }

    route = `/Benefits/InsurancePackages?employerId=${employerId}`;
    fetchRoute(route, onSuccess, onFailure);
}