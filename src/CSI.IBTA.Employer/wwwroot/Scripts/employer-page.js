function showEmployerProfile(employerId) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = `/Employer/${employerId}`;
    fetchRoute(route, onSuccess, null);
}

function showEmployerProfileEditForm(employerId) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = `/Employer/ProfileForm/${employerId}`;
    fetchRoute(route, onSuccess, null);
}

function updateFileName(event) {
    const input = event.target;
    const fileNameDisplay = document.getElementById("fileNameDisplay");

    if (input.files && input.files[0]) {
        const fileName = input.files[0].name;
        fileNameDisplay.textContent = fileName;
    } else {
        fileNameDisplay.textContent = "";
    }
}

function previewImage(event) {
    var reader = new FileReader();
    reader.onload = function () {
        var base64String = reader.result;

        let preview = document.getElementById('imagePreview');
        preview.src = base64String;

        preview.style.display = "block";
    };
    reader.readAsDataURL(event.target.files[0]);
}

function handleEmployerProfileFormSubmit() {
    var form = document.getElementById('employer-profile-form');

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);
    fetch('/Employer', {
        method: 'PUT',
        body: formData,
    })
        .then(function (response) {
            return response.json();
        })
        .then(function (data) {
            if (data.result == null) {
                var errors = document.getElementById('employer-profile-form-errors');
                errors.textContent = data.error.title;
            }
            else {
                showEmployerProfile(data.result.id)
            }
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}