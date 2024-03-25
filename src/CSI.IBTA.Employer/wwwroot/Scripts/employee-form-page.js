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

    let isnum = /^\d+$/.test(formData.get('Phone'));
    let is5num = /^\d{6}$/.test(formData.get('ZipCode'));

    document.getElementById('validate-phone').innerHTML = isnum ? "" : "Phone number has to be digits only";
    document.getElementById('validate-zip').innerHTML = is5num ? "" : "Zip code has to be 6 digits";

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    if (!isnum || !is5num)
        return;

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

            return response.text();
        })
        .then(function (data) {
            //show ok modal
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employee-errors", error);
        });
}