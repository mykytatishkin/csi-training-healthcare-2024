function showTable() {
    $("#table-employer").show();
    $("#control-employer").hide();
}

function showControl(id, name) {
    console.log("ShowControl(" + id + "," + name + ")")
    $("#table-employer").hide();
    $("#control-employer").show();
    let a = document.getElementById("employer-id");
    a.textContent = name;
}

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