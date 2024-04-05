function showSettings(employerId) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = '/Settings?employerId=' + employerId;
    fetchRoute(route, onSuccess, null);
}

function saveSettings() {
    var form = document.getElementById('employer-settings-form');
    var formData = new FormData(form);
    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    fetch(`/Settings/`, {
        method: 'PUT',
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
            showEmployees(formData.get('EmployerId'));
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            let isServerException = error.name == "SyntaxError"
            let errorMsg = !isServerException ? error.message : "Server error"
            showError("setting-errors", errorMsg);
        });
}

function onFollowAdminSettingClick() {
    event.preventDefault();
    const newValue = event.target.value == "True" ? true : false;
    for (let i = 0; i < document.getElementsByName('EmployerAdminCondition').length; i++) {
        let element = document.getElementsByName('EmployerAdminCondition')[i]
        element.disabled = newValue;
    }
}

function onAnySettingClick() {
    event.preventDefault();
    document.getElementById('submit-button').disabled = false;
}