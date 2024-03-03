function showCreatePlanSection(employerId, planId) {
    if (userOperation == "create") {
        document.getElementById('insurance-create-section').innerHTML = null;
        userOperation = "";
        return;
    }

    function onSuccess(data) {
        document.getElementById('insurance-create-section').innerHTML = data;
        userOperation = "create";
    }

    function onFailure(statusCode) {
        showError("employer-package-management-errors", "There was an error, try again");
    }

    route = '/Benefits/CreatePlan?employerId=' + employerId + '&planId=' + planId
    fetchRoute(route, onSuccess, onFailure);
}

function savePlanData() {
    var form = document.getElementById('package-plan-create-form');

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);

    fetch(`/Benefits/${formData.get('ActionName')}?employerId=${formData.get('EmployerId')}&planId=${formData.get('PlanId') }`, {
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
            showEmployerAdministration(formData.get('EmployerId'));
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}