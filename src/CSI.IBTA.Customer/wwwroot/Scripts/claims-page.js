function showClaims() {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/Claims';
    fetchRoute(route, onSuccess, null);
}

function dataValidation() {
    var plan = document.getElementById('plan').value;
    var dateOfService = document.getElementById('date-of-service').value;
    var amount = document.getElementById('amount').value;
    var receipt = document.getElementById('receipt').value;

    var errors = [];

    if (plan === "") {
        errors.push("Please select a plan.");
    }

    if (dateOfService === "") {
        errors.push("Please enter the date of service.");
    }

    if (amount === "") {
        errors.push("Please enter the amount.");
    }

    if (receipt === "") {
        errors.push("Please upload a receipt.");
    }

    if (errors.length > 0) {
        var errorContainer = document.getElementById('file-claim-errors');
        errorContainer.innerHTML = errors.join('<br>');
        return false; // Prevent form submission
    }

    // If all data is valid, you can proceed with form submission or other actions
    console.log("Form data is valid. Submitting form...");
    // document.getElementById('file-claim-form').submit(); // Uncomment this line to submit the form
}