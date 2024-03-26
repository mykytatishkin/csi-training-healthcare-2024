
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

function showCreatePlanForm() {
    event.preventDefault();
    var form = document.getElementById('insurance-package-form');
    var formData = new FormData(form);
    fetch(`/InsurancePlans/OpenCreatePlanForm`, {
        method: 'POST',
        body: formData,
    })
        .then(function (response) {
            if (!response.ok) {
                callbackFailure?.(response.status);
                throw new Error("Response was not ok");
            }

            if (response.redirected) {
                window.location.href = response.url;
                return;
            }

            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}

function showUpdatePlanForm(planIndex) {
    event.preventDefault();
    var form = document.getElementById('insurance-package-form');
    var formData = new FormData(form);
    formData.append('PlanForm.SelectedPlanIndex', planIndex);

    fetch(`/InsurancePlans/OpenUpdatePlanForm`, {
        method: 'POST',
        body: formData,
    })
        .then(function (response) {
            if (!response.ok) {
                callbackFailure?.(response.status);
                throw new Error("Response was not ok");
            }

            if (response.redirected) {
                window.location.href = response.url;
                return;
            }

            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}

function handlePackagePlanFormLeave() {
    var form = document.getElementById('package-plan-form');
    var formData = new FormData(form);

    fetch(`/InsurancePlans/HandlePackagePlanFormCancel`, {
        method: 'POST',
        body: formData,
    })
        .then(function (response) {
            if (!response.ok) {
                return response.json().then(function (json) {
                    throw new Error(json.title);
                });
            }
            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}

function upsertPlan() {
    var form = document.getElementById('package-plan-form');

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);

    fetch(`/InsurancePlans`, {
        method: 'PUT',
        body: formData,
    })
        .then(function (response) {
            if (!response.ok) {
                return response.json().then(function (json) {
                    throw new Error(json.title);
                });
            }
            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}
