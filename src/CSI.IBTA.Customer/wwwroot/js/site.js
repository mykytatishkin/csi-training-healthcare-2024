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

function showEmployerLogo() {
    function onSuccess(data) {
        document.getElementById('employer-logo').src = "data:image/png;base64," + data;
    }

    route = `/EmployerLogo`;
    fetchRoute(route, onSuccess, null);
}