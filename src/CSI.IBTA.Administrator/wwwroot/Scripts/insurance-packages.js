function submitPackageCreation() {
    var form = document.getElementById('insurance-package-form');

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);

    var planStart = formData.get('Package.PlanStart')
    var planEnd = formData.get('Package.PlanEnd')

    if (planStart >= planEnd) {
        showError("create-insurance-package-errors", "Plan start date cannot be later than plan end date or at the same day");
        return;
    }

    var employerId = formData.get('EmployerId')

    fetch('/InsurancePackage', {
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
            showEmployerPackagePlans(employerId);
        })
        .catch(function (error) {
            if (error.message.includes("Insurance package with this name already exists")) {
                showError("create-insurance-package-errors", "An insurance package with this name already exists. Please choose a different name.");
            } else {
                showError("create-insurance-package-errors", error.message);
            }
            console.error('There was a problem with the fetch operation:', error);
        });
}

function submitPackageUpdate() {
    var form = document.getElementById('insurance-package-form');

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);

    var planStart = formData.get('Package.PlanStart')
    var planEnd = formData.get('Package.PlanEnd')

    if (planStart >= planEnd) {
        showError("create-insurance-package-errors", "Plan start date cannot be later than plan end date or at the same day");
        return;
    }

    var employerId = formData.get('EmployerId')

    fetch('/InsurancePackage/UpdateInsurancePackage', {
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
            showEmployerPackagePlans(employerId);
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("create-insurance-package-errors", error);
        });
}

function createInsurancePackage(employerId) {
    fetch('/InsurancePackage?employerId=' + employerId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function modifyInsurancePackage(insurancePackageId, employerId) {
    fetch('/InsurancePackage/UpdateInsurancePackage?employerId=' + employerId + '&insurancePackageId=' + insurancePackageId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
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