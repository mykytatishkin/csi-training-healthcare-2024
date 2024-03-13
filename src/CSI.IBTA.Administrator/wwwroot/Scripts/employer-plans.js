
function showEmployerPackagePlans(employerId) {
    function onSuccess(data) {
        document.getElementById('employer-partial-action').innerHTML = data;
    }

    function onFailure(statusCode) {
        console.error('There was a problem with the fetch operation:', statusCode);
        showError("employer-administration-errors", "Server error");
    }

    route = `/InsurancePackage/InsurancePackages?employerId=${employerId}`;
    fetchRoute(route, onSuccess, onFailure);
}

function initializePackage(packageId, employerId) {
    fetch(`/InsurancePackage/InitializePackage?packageId=${packageId}&employerId=${employerId}`, {
        method: 'PATCH'
    })
        .then(function (response) {
            if (!response.ok) {
                showError("employer-package-errors", "Failed to initialize insurance package");
                return;
            }
            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })

}

function removePackage(packageId, employerId) {
    fetch(`/InsurancePackage/RemovePackage?packageId=${packageId}&employerId=${employerId}`, {
        method: 'DELETE'
    })
        .then(function (response) {
            if (!response.ok) {
                showError("employer-package-errors", "Failed to remove insurance package");
                return;
            }
            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
}