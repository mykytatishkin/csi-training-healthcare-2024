function showEmployerAdministration(employerId) {
    fetch('/Employer/AdministrationMenu?employerId=' + employerId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
                showError("employer-user-management-errors", "There was an error, try again");
                return;
            }

            return response.text();
        })
        .then(function (data) {
        document.getElementById('employer-partial-action').innerHTML = data;
        selectedUserRowElement = null;
        selectedUserId = null;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        showError("employer-user-management-errors", "There was an error, try again");
        });
    }

    route = '/Employer/AdministrationMenu?employerId=' + employerId;
    fetchRoute(route, onSuccess, onFailure);
}

function showEmployerUsersManagement(employerId) {
    fetch('/Employer/Users?employerId=' + employerId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
                showError("employer-administration-menu-errors", "There was an error, try again");
                return;
    }

            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        showError("employer-administration-menu-errors", "There was an error, try again");
        });
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

function showUserInformationSection() {
    var element = document.querySelector('.user-information-section');
    if (element.hasAttribute('hidden')) {
        element.removeAttribute('hidden');
    } else {
        element.setAttribute('hidden', 'true');
    }
}

let selectedUserId = -1;
let selectedUserRowElement = null;
let updateButton = null;

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
    fetch('/Employer/CreateUser?employerId=' + employerId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
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
            document.getElementById('user-create-section').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", "There was an error, try again");
        });
}

function showUpdateUserSection(employerId) {
    fetch('/Employer/UpdateUser?employerId=' + employerId + '&userId=' + selectedUserId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
                showError("employer-user-management-errors", "There was an error, try again");
                return;
            }

            return response.text();
        })
        .then(function (data) {
            document.getElementById('user-create-section').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", "There was an error, try again");
        });
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
                throw new Error('Network response was not ok');
                showError("employer-user-management-errors", "There was an error, try again");
                return;
            }

            showEmployerUsersManagement(formData.get('EmployerId'));
            deselectEmployerUserRow();
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", "There was an error, try again");
        });
}