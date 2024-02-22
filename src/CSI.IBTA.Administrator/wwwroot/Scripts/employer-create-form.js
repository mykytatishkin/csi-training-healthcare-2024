function handleEmployerFormSubmit() {
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
        if (!response.ok) {
            var errors = document.getElementById('employer-form-errors');
            errors.textContent = "There was a problem with the fetch operation";
        }

        return response.json();
    })
    .then(function (data) {
        if (!data.success) {
            var errors = document.getElementById('employer-form-errors');
            errors.textContent = data.errorMessage;
        }
        else {
            document.getElementById('control-employer-form').style.display = "none";
            document.getElementById('table-employer').style.display = "block";
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
        var output = document.getElementById('imagePreview');
        output.src = reader.result;
        output.style.display = 'block';
        var output2 = document.getElementById('imagePreview2');
        output2.src = reader.result;
        output2.style.display = 'block';
    };
    reader.readAsDataURL(event.target.files[0]);
}

function showEmployerForm() {
    fetch(`/Employer/EmployerForm`)
    .then(function (response) {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.text();
    })
    .then(function (data) {
        document.getElementById('control-employer-form').innerHTML = data;
        $("#table-employer").hide();
        $("#control-employer-form").show();
    })
    .catch(function (error) {
        console.error('There was a problem with the fetch operation:', error);
    });
}