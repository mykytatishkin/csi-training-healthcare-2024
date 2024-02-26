function showEmployerAdministration(employerId) {
    function onSuccess(data) {
        document.getElementById('employer-partial-action').innerHTML = data;
        selectedUserRowElement = null;
        selectedUserId = null;
    }

    function onFailure(statusCode) {
        showError("employer-user-management-errors", "There was an error, try again");
    }

    route = '/Employer/AdministrationMenu?employerId=' + employerId;
    fetchRoute(route, onSuccess, onFailure);
}

function showEmployerUsersManagement(employerId) {
    function onSuccess(data) {
        document.getElementById('employer-partial-action').innerHTML = data;
    }

    function onFailure(statusCode) {
        showError("employer-administration-menu-errors", "There was an error, try again");
    }

    route = '/Employer/Users?employerId=' + employerId;
    fetchRoute(route, onSuccess, onFailure);
}

function showError(id, text) {
    document.getElementById(id).innerText = text;
}

function generateRandomPassword(length) {
    const charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!$%^&*()-_=+[]{}|;:'\",.<>/?";
    let password = "";
    for (let i = 0; i < length; i++) {
        const randomIndex = Math.floor(Math.random() * charset.length);
        password += charset[randomIndex];
    }
    return password;
}

function setRandomPassword() {
    const passwordLength = 12;
    const password = generateRandomPassword(passwordLength);
    const passwordInput = document.getElementById("passwordInput");

    if (passwordInput) {
        passwordInput.value = password;
    } else {
        console.error("Input field not found!");
    }
}

let selectedUserId = -1;
let selectedUserRowElement = null;
let userOperation = "";

function onEmployerUserRowClick(element) {
    updateButton = document.getElementById('update-button');
    currentId = element.getAttribute('data-userId');

    if (selectedUserId == currentId) {
        deselectEmployerUserRow();
    } else {
        selectEmployerUserRow(element);
    }

    if (userOperation == "update") {
        hideUserSection();
    }
}

function selectEmployerUserRow(element) {
    updateButton = document.getElementById('update-button');

    if (selectedUserRowElement != null) {
        selectedUserRowElement.classList.remove("highlight");
    }

    element.classList.add("highlight");
    selectedUserId = element.getAttribute('data-userId');
    selectedUserRowElement = element;
    updateButton.removeAttribute('hidden');
}

function deselectEmployerUserRow() {
    if (selectedUserRowElement != null) {
        updateButton = document.getElementById('update-button');
        updateButton.setAttribute('hidden', true);

        selectedUserRowElement.classList.remove("highlight");
        selectedUserId = -1;
        selectedUserRowElement = null;
    }
}

function hideUserSection() {
    document.getElementById('user-create-section').innerHTML = null;
}

function showCreateUserSection(employerId) {
    if (userOperation == "create") {
        document.getElementById('user-create-section').innerHTML = null;
        userOperation = "";
        return;
    }

    function onSuccess(data) {
        document.getElementById('user-create-section').innerHTML = data;
        userOperation = "create";
    }

    function onFailure(statusCode) {
        showError("employer-user-management-errors", "There was an error, try again");
    }

    route = '/Employer/CreateUser?employerId=' + employerId
    fetchRoute(route, onSuccess, onFailure);
}

function showUpdateUserSection(employerId) {
    function onSuccess(data) {
        document.getElementById('user-create-section').innerHTML = data;
        userOperation = "update";
    }

    function onFailure(statusCode) {
        showError("employer-user-management-errors", "There was an error, try again");
    }

    route = '/Employer/UpdateUser?employerId=' + employerId + '&userId=' + selectedUserId
    fetchRoute(route, onSuccess, onFailure);
}

function saveUserData() {
    var form = document.getElementById('employer-user-create-form');

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);

    fetch(`/Employer/${formData.get('ActionName')}?employerId=${formData.get('EmployerId')}&userId=${selectedUserId}`, {
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
            showEmployerUsersManagement(formData.get('EmployerId'));
            deselectEmployerUserRow();
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}

function saveSettings() {
    var form = document.getElementById('employer-settings-form');
    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }
    var formData = new FormData(form);
    fetch('/Employer/AllSettings?employerId=' + formData.get('EmployerId'), {
        method: 'PATCH',
        body: formData,
    })
        .then(function (response) {
            if (!response.ok) {
                return response.json().then(function (json) {
                    throw new Error(json.title);
                });
            }

            showEmployerUsersManagement(formData.get('EmployerId'));
            deselectEmployerUserRow();
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}

function saveSettings() {
    var form = document.getElementById('employer-settings-form');
    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }
    var formData = new FormData(form);
    fetch('/Employer/AllSettings?employerId=' + formData.get('EmployerId'), {
        method: 'PATCH',
        body: formData,
    })
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            showEmployerAdministration(formData.get('EmployerId'));
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}