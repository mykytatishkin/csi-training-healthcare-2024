function showClaims(employeeId, employerId) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = `/Claims?employeeId=${employeeId}&employerId=${employerId}`;
    fetchRoute(route, onSuccess, null);
}

function handleCancel() {
    document.getElementById('file-claim-form').reset();
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

function saveFileClaimData() {
    dataValidation();
    var form = document.getElementById('file-claim-form');
    var formData = new FormData(form);

    if (form.checkValidity() == false) {
        return;
    }

    fetch(`/Claims`, {
        method: 'POST',
        body: formData,
    })
        .then(function (response) {
            if (!response.ok) {
                return response.json().then(function (json) {
                    throw new Error(json.title);
                });
            }
        })
        .then(function (data) {
            showClaims();
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });

}