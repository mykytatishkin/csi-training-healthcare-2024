function openEmployeeForm(employerId) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/CreateEmployee?' + new URLSearchParams({
        employerId: employerId
    });

    fetchRoute(route, onSuccess, null);
}


function saveEmployeeData() {
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
        return;
    }

    fetch(`/${formData.get('ActionName')}`, {
        method: 'POST',
        body: formData,
    })
        .then(function (response) {
            if (!response.ok) {
                return response.json().then(function (json) {
                    throw new Error(json.title);
                });
            }

            //return response.text();
        })
        .then(function (data) {
            showModal('confirmModal')
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employee-errors", error);
        });
}