function showClaimDetails(claimId) {
    console.log("aaa");

    function onSuccess(data) {
        console.log(document.getElementById('claims-view'));
        document.getElementById('claims-view').innerHTML = data;
    }

    function onFailure(statusCode) {
        showError("claim-list-error", "Failed to fetch claim, try again");
    }

    route = `/Claims/Details?claimId=${claimId}`;
    fetchRoute(route, onSuccess, onFailure);
}

function showModal(modalId) {
    var modal = document.getElementById(modalId);
    modal.style.display = "block";
}

function closeModal(modalId) {
    var modal = document.getElementById(modalId);
    modal.style.display = "none";
}