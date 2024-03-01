function submitPackageCreation() {
    var form = document.getElementById('insurance-package-create-form');

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
            // Waiting for blocker task to be finished, because there is nowhere to go back
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("create-insurance-package-errors", error);
        });
}

function cancelPackageCreation() {
    // Waiting for blocker task to be finished, because there is nowhere to go back
}