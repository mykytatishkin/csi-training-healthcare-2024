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
    if (encodedReceipt == null || encodedReceipt == "") {
        return;
    }

    if (isBase64PDF(encodedReceipt)) {
        fetch('Claims/PdfReceipt', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ encodedReceipt })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to load PDF');
                }
                return response.blob();
            })
            .then(blob => {
                return new Promise((resolve, reject) => {
                    const reader = new FileReader();
                    reader.onloadend = () => resolve(reader.result);
                    reader.onerror = reject;
                    reader.readAsDataURL(blob);
                });
            })
            .then(dataUrl => {
                document.getElementById('pdf-viewer').setAttribute('src', dataUrl);
            })
            .catch(error => {
                console.error('Error loading PDF:', error);
            });

        return;
    }

    try {
        window.atob(encodedReceipt);
        document.getElementById('img-receipt').src = "data:image/png;base64," + encodedReceipt;
        return;
    } catch (e) { }
}

function isBase64PDF(encodedData) {
    const byteCharacters = atob(encodedData);
    const header = byteCharacters.slice(0, 4);
    return header === "%PDF";
}