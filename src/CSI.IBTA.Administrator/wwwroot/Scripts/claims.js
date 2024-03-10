function showEditClaim(claimId) {
    function onSuccess(data) {
        document.getElementById('control-claim').innerHTML = data;
        $("#table-employer").hide();
        $("#control-employer").show();
    }

    function onFailure(statusCode) {
        console.error('There was a problem with the fetch operation:', statusCode);
    }

    route = `/Benefits/EditClaim?claimId=${claimId}`;
    fetchRoute(route, onSuccess, onFailure);
}

function saveClaimData() {
    var form = document.getElementById('claim-edit-form');
    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }
    var formData = new FormData(form);
    fetch(`/Benefits/EditClaim?claimId=${formData.get('ClaimId')}`, {
        method: 'PATCH',
        body: formData,
    })
        .then(function (response) {
            if (!response.ok) {
                return response.json().then(function (json) {
                    throw new Error(json.title);
                });
            }

            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}