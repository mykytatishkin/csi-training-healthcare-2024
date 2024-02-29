function showCreatePlanSection(employerId) {
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

    route = '/Benefits/CreatePlan?employerId=' + employerId
    fetchRoute(route, onSuccess, onFailure);
}