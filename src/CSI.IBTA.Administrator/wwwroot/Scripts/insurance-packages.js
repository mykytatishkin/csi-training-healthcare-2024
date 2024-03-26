function upsertInsurancePackage() {
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

    fetch('/InsurancePackage/UpsertInsurancePackage', {
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
            var employerId = formData.get('EmployerId')
            showEmployerPackagePlans(employerId);
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("create-insurance-package-errors", error.name == "SyntaxError" ? "Failed due to server error" : error.message);
        });
}

function openCreateInsurancePackageForm(employerId) {
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

function openModifyInsurancePackageForm(insurancePackageId, employerId) {
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