function submitPackageCreation() {
    var form = document.getElementById('insurance-package-create-form');

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);

    fetch('/Insurance/CreateInsurancePackage', {
        method: 'POST',
        body: formData,
    })
        .then(function (response) {
            if (!response.ok) {
                console.log(response)
                return response.json().then(function (json) {
                    throw new Error(json.title);
                });
            }

            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}

function cancelPackageCreation() {

}