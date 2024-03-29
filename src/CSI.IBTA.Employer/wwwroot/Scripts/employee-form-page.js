function openEmployeeForm(employerId) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/Employees/CreateEmployee?' + new URLSearchParams({
        employerId: employerId
    });

    fetchRoute(route, onSuccess, null);
}


function setSubmit(operationStart) {
    if (operationStart) {
        document.getElementById('submit-button').disabled = true;
        document.getElementById('submit-button').innerText = "Creating Employee...";
    } else {
        document.getElementById('submit-button').disabled = false;
        document.getElementById('submit-button').innerText = "Submit";
    }
}
function saveEmployeeData() {
    setSubmit(true);
    var form = document.getElementById('employee-upsert-form');
    var formData = new FormData(form);

    let phoneIsDigits = /^\d+$/.test(formData.get('Phone'));
    let zipIs6Digits = /^\d{6}$/.test(formData.get('ZipCode'));

    if (!phoneIsDigits) {
        document.getElementById('Phone').setCustomValidity("Phone number has to be not empty and digits only");
    }
    if (!zipIs6Digits) {
        document.getElementById('ZipCode').setCustomValidity("Zip code has to be not empty and 6 digits");
    }
    if (form.checkValidity() == false) {
        form.reportValidity();
        setSubmit(false);
        return;
    }

    fetch(`/Employees/${formData.get('ActionName')}`, {
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
            showError("employee-errors", "");
            showModal('confirmModal')
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            let isServerException = error.name == "SyntaxError"
            let errorMsg = !isServerException ? error.message : "Server error"
            showError("employee-errors", errorMsg);
        })
        .finally(function () {
            setSubmit(false);
        });
}
