function showClaimDetails(claimId) {

    function onSuccess(data) {
        document.getElementById('claims-view').innerHTML = data;
    }

    function onFailure(statusCode) {
        showError("claim-list-error", "Failed to fetch claim, try again");
    }

    route = `/Claims/Details?claimId=${claimId}`;
    fetchRoute(route, onSuccess, onFailure);
}

function handleApproveClaim(claimId) {
    fetch(`/Claims/Approve/${claimId}`, {
        method: 'PATCH'
    })
        .then(function (response) {
            return response.json();
        })
        .then(function (data) {
            if (data.result == false) {
                var errors = document.getElementById('modal-form-errors');
                errors.textContent = data.error.title;
            }
            else {
                closeModal('approveModal');
                showClaims();
            }
        })
        .catch(function (error) {
            console.error('There was a problem with the handleApproveClaim operation:', error);
            showError("modal-form-errors", "Server failed to modify the claim, try again");
        });
}

function handleDenyClaim(claimId) {
    var form = document.getElementById('deny-claim-form');
    if (form.checkValidity() == false) {
        form.reportValidity();
        return;
    }

    var formData = new FormData(form);
    fetch(`/Claims/Deny/${claimId}`, {
        method: 'PATCH',
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
                closeModal('denyModal');
                showClaims();
            }
        })
        .catch(function (error) {
            console.error('There was a problem with the handleDenyClaim operation:', error);
            showError("modal-form-errors", "Server failed to modify the claim, try again");
        });
}

function showReceiptModal(encodedReceipt) {
    showReceipt(encodedReceipt);
    showModal('receiptModal');
}

function showReceipt(encodedReceipt) {
    if (encodedReceipt != null && encodedReceipt != "") {
        try {
            window.atob(encodedReceipt);
            document.getElementById('img-receipt').src = "data:image/png;base64," + encodedReceipt;
            return;
        } catch (e) { }
    }
}