function showEditClaim() {
    var form = document.getElementById('claim-view-form');
    var formData = new FormData(form);
    route = `/Claims/OpenEditClaim`;
    fetch(route, {
        method: 'POST',
        body: formData,
    })
        .then(function (response) {
            if (!response.ok) {
                callbackFailure?.(response.status);
                throw new Error("Response was not ok");
            }

            if (response.redirected) {
                window.location.href = response.url;
                return;
            }

            return response.text();
        })
        .then(function (data) {
            console.log(document.getElementById('claims-view'));
            document.getElementById('claims-view').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}

function saveClaimData() {
    var form = document.getElementById('claim-edit-form');
    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }
    var formData = new FormData(form);
    fetch(`/Claims/EditClaim?claimId=${formData.get('Claim.Id')}`, {
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
            document.getElementById('claims-view').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}

function handleEditClaimCancel(claimId) {
    event.preventDefault();
    showClaimDetails(claimId);
}
