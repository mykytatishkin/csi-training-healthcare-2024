function showEmployerAdministration(employerId) {
    fetch('/Employer/AdministrationMenu?employerId=' + employerId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
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
        });
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

function highlightEmployerUserRow(element) {
    const updateButton = document.getElementById('update-button');
    currentId = element.getAttribute('data-userId');

    if (selectedUserId == currentId) {
        updateButton.setAttribute('hidden', true);
        element.classList.remove("highlight");
        selectedUserId = -1;
        selectedUserRowElement = null;
    } else {
        if (selectedUserRowElement != null) {
            selectedUserRowElement.classList.remove("highlight");
        }
        element.classList.add("highlight");
        selectedUserId = element.getAttribute('data-userId');
        selectedUserRowElement = element;
        updateButton.removeAttribute('hidden');
    }
}

function showCreateUserSection(employerId) {
    fetch('/Employer/CreateUser?employerId=' + employerId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(function (data) {
            document.getElementById('user-create-section').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function showUpdateUserSection(employerId) {
    fetch('/Employer/UpdateUser?employerId=' + employerId + '&userId=' + selectedUserId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(function (data) {
            document.getElementById('user-create-section').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function saveUserData() {
    var form = document.getElementById('employer-user-create-form');

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);

    fetch('/Employer/' + formData.get('ActionName') + '?employerId=' + formData.get('EmployerId') + '&userId=' + selectedUserId, {
        method: 'POST',
        body: formData,
    })
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            showEmployerUsersManagement(formData.get('EmployerId'));
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function saveSettings(employerId) {
    var form = document.getElementById('employer-settings-form');
    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }
    var formData = new FormData(form);
    fetch('/Employer/AllSettings?employerId=' + employerId, {
        method: 'PATCH',
        body: formData,
    })
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            showEmployerSettings(employerId);
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}