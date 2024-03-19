
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

function showAddPlanForm() {
    var form = document.getElementById('insurance-package-form');
    var formData = new FormData(form);
    event.preventDefault();
    fetch(`/InsurancePlans/OpenAddPlanToListForm`, {
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
            let btn = document.getElementById("plan-form-submit-btn");
            btn.addEventListener("click", saveNewPlanData);
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}

function showUpdatePlanForm(planIndex) {
    var form = document.getElementById('insurance-package-form');
    var formData = new FormData(form);
    event.preventDefault();
    formData.append('SelectedPlanIndex', planIndex);

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
            let btn = document.getElementById("plan-form-submit-btn");
            btn.addEventListener("click", updatePlanData);
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}

function showUpdatePackageAddPlanForm() {
    event.preventDefault();
    var form = document.getElementById('insurance-package-form');
    var formData = new FormData(form);
    fetch(`/InsurancePlans/OpenUpdatePlanToListForm`, {
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
            let btn = document.getElementById("plan-form-submit-btn");
            btn.addEventListener("click", saveNewUpdatePlanData);
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}

function showUpdatePackageUpdatePlanForm(planIndex) {
    event.preventDefault();
    var form = document.getElementById('insurance-package-form');
    var formData = new FormData(form);
    formData.append('SelectedPlanIndex', planIndex);

    fetch(`/InsurancePlans/OpenUpdatePackageUpdatePlanForm`, {
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
            let btn = document.getElementById("plan-form-submit-btn");
            btn.addEventListener("click", saveUpdatedPlanData);
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}

function handleCreatePackagePlanFormLeave() {
    var form = document.getElementById('package-plan-add-form');
    var formData = new FormData(form);

    fetch(`/InsurancePlans/HandleCreatePackagePlanFormCancel`, {
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

function handleUpdatePackagePlanFormLeave() {
    var form = document.getElementById('package-plan-add-form');
    var formData = new FormData(form);

    fetch(`/InsurancePlans/HandleUpdatePackagePlanFormCancel`, {
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
