function showEmployerDetails(employerId, encodedLogo) {
    function onSuccess(data) {
        document.getElementById('control-employer').innerHTML = data;
        $("#table-employer").hide();
        $("#control-employer").show();

        console.log("onSuccess()")
    }

    function onFailure(statusCode) {
        console.error('There was a problem with the fetch operation:', statusCode);
    }

    route = `/Employer?employerId=${employerId}`;
    fetchRoute(route, onSuccess, onFailure);
    getEncodedLogo(encodedLogo);
    console.log("getEncodedLogo()")
    console.log(encodedLogo)

}

function showEmployerSettings(employerId) {
    fetch('/Employer/AllSettings?employerId=' + employerId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function createInsurancePackage(employerId) {
    fetch('/InsurancePackage?employerId=' + employerId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function modifyInsurancePackage(insurancePackageId, employerId) {
    fetch('/InsurancePackage/UpdateInsurancePackage?employerId=' + employerId + '&insurancePackageId=' + insurancePackageId)
        .then(function (response) {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(function (data) {
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function showAddPlanForm() {
    var form = document.getElementById('insurance-package-create-form');
    var formData = new FormData(form);
    fetch(`/InsurancePlans/OpenAddPlanToListForm`, {
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
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}

function showUpdatePlanForm() {
    var form = document.getElementById('insurance-package-create-form');
    var formData = new FormData(form);
    fetch(`/InsurancePlans/OpenUpdatePlanToListForm`, {
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
            document.getElementById('employer-partial-action').innerHTML = data;
        })
        .catch(function (error) {
            console.error('There was a problem with the fetch operation:', error);
            showError("employer-user-management-errors", error);
        });
}