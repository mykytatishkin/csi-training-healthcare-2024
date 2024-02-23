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
        return response.json();
    })
        .then(function (data) {
            if (data.value == null) {
            var errors = document.getElementById('employer-form-errors');
            errors.textContent = data.description;
        }
        else {
                showEmployerDetails(data.value.id);
            //fetch('/Employer/AdministrationMenu?employerId=' + data.value.id)
            //    .then(function (response) {
            //        if (!response.ok) {
            //            throw new Error('Network response was not ok');
            //            showError("employer-user-management-errors", "There was an error, try again");
            //            return;
            //        }

            //        return response.text();
            //    })
            //    .then(function (data) {
            //        var e = document.getElementById('control-employer');
            //        console.error(e);
            //        document.getElementById('control-employer').innerHTML = data;
            //    })
            //    .catch(function (error) {
            //        console.error('There was a problem with the fetch operation:', error);
            //        showError("employer-user-management-errors", "There was an error, try again");
            //    });
            
            //document.getElementById('control-employer-form').style.display = "none";
            //document.getElementById('table-employer').style.display = "block";
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
            let form = document.getElementById('control-employer');
            form.innerHTML = data;
            $("#control-employer").show();

        $("#table-employer").hide();
    })
    .catch(function (error) {
        console.error('There was a problem with the fetch operation:', error);
    });
}