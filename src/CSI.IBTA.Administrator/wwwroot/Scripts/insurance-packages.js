﻿function submitPackageCreation() {
    console.log('???');
    var form = document.getElementById('insurance-package-create-form');

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);

    console.log(JSON.stringify(formData));

    var planStart = formData.get('Package.PlanStart')
    var planEnd = formData.get('Package.PlanEnd')

    if (planStart >= planEnd) {
        showError("create-insurance-package-errors", "Plan start date cannot be later than plan end date or at the same day");
        return;
    }

    var employerId = formData.get('EmployerId')
    console.log(employerId);

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
            console.log("okay???");
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
    console.log('???');
    var form = document.getElementById('insurance-package-create-form');

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);

    console.log(JSON.stringify(formData));

    var planStart = formData.get('Package.PlanStart')
    var planEnd = formData.get('Package.PlanEnd')

    if (planStart >= planEnd) {
        showError("create-insurance-package-errors", "Plan start date cannot be later than plan end date or at the same day");
        return;
    }

    var employerId = formData.get('EmployerId')
    console.log(employerId);

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
            console.log("okay???");
            showEmployerPackagePlans(employerId);
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("create-insurance-package-errors", error);
        });
}