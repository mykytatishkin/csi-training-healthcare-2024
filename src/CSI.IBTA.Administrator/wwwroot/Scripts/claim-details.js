function showClaimDetails(claimId) {

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