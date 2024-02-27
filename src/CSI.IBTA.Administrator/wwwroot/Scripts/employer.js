function showEmployerDetails(employerId) {
    function onSuccess(data) {
        document.getElementById('control-employer').innerHTML = data;
        $("#table-employer").hide();
        $("#control-employer").show();
    }

    function onFailure(statusCode) {
        console.error('There was a problem with the fetch operation:', statusCode);
    }

    route = `/Employer?employerId=${employerId}`;
    fetchRoute(route, onSuccess, onFailure);
}