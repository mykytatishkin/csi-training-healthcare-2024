function showEmployerDetails(employerId) {
    function onSuccess(data) {
            document.getElementById('control-employer').innerHTML = data;
            $("#table-employer").hide();
            $("#control-employer").show();
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}

    function onFailure(statusCode) {
        console.error('There was a problem with the fetch operation:', statusCode);
            }

    route = `/Employer?employerId=${employerId}`;
    fetchRoute(route, onSuccess, onFailure);
}