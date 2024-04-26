function showClaims(employeeId, employerId, pageNumber) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/Claims?' + new URLSearchParams({
        pageNumber: pageNumber,
        employeeId: employeeId,
        employerId: employerId,
    });
    fetchRoute(route, onSuccess, null);
}

function updateFileName(event) {
    var input = event.target;
    var fileName = input.files[0].name;
    document.getElementById('fileNameDisplay').innerText = fileName;
}

function showFileClaim(employeeId, employerId, enrollmentId) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = `/Claims/FileClaim?employeeId=${employeeId}&employerId=${employerId}&enrollmentId=${enrollmentId}`;
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

    if (amount <= 0) {
        errors.push("Amount should be greater than 0.");
    }

    if (receipt === "") {
        errors.push("Please upload a receipt.");
    }

    if (errors.length > 0) {
        var errorContainer = document.getElementById('file-claim-errors');
        errorContainer.innerHTML = errors.join('<br>');
        return false; // Prevent form submission
    }
}

function saveFileClaimData() {
    var errorContainer = document.getElementById('file-claim-errors');
    errorContainer.innerHTML = ''

    let validationRes = dataValidation();
    var form = document.getElementById('file-claim-form');
    var formData = new FormData(form);

    if (validationRes == false) {
        return;
    }

    fetch(`/Claims/FileClaim`, {
        method: 'POST',
        body: formData,
    })
    .then(function (response) {
        return response.json();
    })
    .then(function (data) {
        if (data.result != true) {
            showMessage('file-claim-errors', data.error.title, "red");
        }
        else {
            showModal('confirmModal');
        }
    })
    .catch(function (error) {
        console.error('There was a problem with the fetch operation:', error);
    });

}