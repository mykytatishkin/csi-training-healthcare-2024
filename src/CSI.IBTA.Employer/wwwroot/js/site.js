function fetchRoute(route, callbackSuccess, callbackFailure) {
    fetch(route)
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
            callbackSuccess?.(data);
        })
        .catch(function (error) {
            console.error("Fetch request failed: " + error)
        });
}

function showModal(modalId) {
    var modal = document.getElementById(modalId);
    modal.style.display = "block";
}

function closeModal(modalId) {
    var modal = document.getElementById(modalId);
    modal.style.display = "none";
}

function showError(errorElementId, text) {
    let error = document.getElementById(errorElementId);
    error.innerText = text;
    error.style.display = "block";
}

function hideError(errorElementId) {
    let error = document.getElementById(errorElementId);
    error.innerText = "";
    error.style.display = "none";
}