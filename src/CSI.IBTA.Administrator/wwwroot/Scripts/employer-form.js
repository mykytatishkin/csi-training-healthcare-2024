function handleCreateEmployerFormSubmit() {
    var form = document.getElementById('employer-form');

    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);
    fetch('/Employer', {
        method: 'POST',
        body: formData,
    })
    .then(function (response) {
        return response.json();
    })
    .then(function (data) {
        if (data.result == null) {
        var errors = document.getElementById('employer-form-errors');
        errors.textContent = data.error.title;
    }
    else {
        showEmployerDetails(data.result.id);
    }

    })
    .catch(function (error) {
        console.error('There was a problem with the fetch operation:', error);
    });
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

        let preview2 = document.getElementById('imagePreview2');
        preview2.src = base64String; 

        preview.style.display = "block";
        preview2.style.display = "block";
    };
    reader.readAsDataURL(event.target.files[0]);
}

function showEmployerCreateForm() {
    function onSuccess(data) {
        let form = document.getElementById('control-employer');
        form.innerHTML = data;
        let btn = document.getElementById("employer-form-submit-btn");
        btn.addEventListener("click", handleCreateEmployerFormSubmit);
        $("#control-employer").show();
        $("#table-employer").hide();
    }

    function onFailure(statusCode) {
        console.error('There was a problem with the fetch operation:', statusCode);
    }

    route = `/Employer/CreateEmployerForm`;
    fetchRoute(route, onSuccess, onFailure);
}

function showEmployerUpdateForm(employerId) {
    function onSuccess(data) {
        let form = document.getElementById('control-employer');
        form.innerHTML = data;
        let btn = document.getElementById("employer-form-submit-btn");
        btn.addEventListener("click", handleUpdateEmployerFormSubmit);
        let preview = document.getElementById('imagePreview');
        preview.style.display = "block";

        $("#control-employer").show();
        $("#table-employer").hide();
        $("#logo-employer").hide();
    }

    function onFailure(statusCode) {
        console.error('There was a problem with the fetch operation:', statusCode);
    }

    route = `/Employer/UpdateEmployerForm?employerId=${employerId}`;
    fetchRoute(route, onSuccess, onFailure);
}

function handleUpdateEmployerFormSubmit() {
    var form = document.getElementById('employer-form');

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
            var errors = document.getElementById('employer-form-errors');
            errors.textContent = data.error.title;
        }
        else {
            showEmployerDetails(data.result.id);
        }
    })
    .catch(function (error) {
        console.error('There was a problem with the fetch operation:', error);
    });
}

function handleCancel(employerId)
{
    if (employerId == undefined) {
        hideCreateEmployerForm();
    }
    else {
        showEmployerDetails(employerId);
    }
}

function hideCreateEmployerForm() {
    $("#control-employer").hide();
    $("#table-employer").show();
}