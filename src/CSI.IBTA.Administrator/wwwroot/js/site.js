// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
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

function a(id) {
    alert(id)
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