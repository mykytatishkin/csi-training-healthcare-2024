function showEmployerDetails(employerId) {
    fetch('/Employer?employerId=' + employerId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(function (data) {
            document.getElementById('control-employer').innerHTML = data;
            $("#table-employer").hide();
            $("#control-employer").show();
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function showEmployerUsersManagement(employerId) {
    fetch('/Employer/Users?employerId=' + employerId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}