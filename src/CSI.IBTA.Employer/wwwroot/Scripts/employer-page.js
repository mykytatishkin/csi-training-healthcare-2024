function showEmployerProfile(employerId) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
    }

    route = `/Employer/${employerId}`;
    fetchRoute(route, onSuccess, null);
}

//function addRestoreLogoEvent(btnId, oldLogoSrc) {
//    let btn = document.getElementById(btnId);
//    btn.addEventListener("click", () => 
//        document.getElementById('logo-employer').src = `data:image/png;base64, ${oldLogoSrc}`
//    )
//}

function showEmployerProfileEditForm(employerId, oldLogoSrc) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;

        //addRestoreLogoEvent('profile-form-cancel-btn', oldLogoSrc);
        //addRestoreLogoEvent('home-btn', oldLogoSrc);
        //addRestoreLogoEvent('employees-btn', oldLogoSrc);
        //addRestoreLogoEvent('setup-btn', oldLogoSrc);
        //addRestoreLogoEvent('import-btn', oldLogoSrc);
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
                document.getElementById('logo-employer').src= `data:image/png;base64, ${data.result.encodedLogo}`;
            }
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}