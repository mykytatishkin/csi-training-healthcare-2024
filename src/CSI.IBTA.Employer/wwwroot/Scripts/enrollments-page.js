var fetchedEnrollments = null;
var fetchedPackages = null;
var allPlans = null;
var fetchedEmployerId = -1;
var fetchedEmployeeId = -1;

async function showEnrollments(employerId, employeeId) {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
        handleEnrollmentsSection();
    }

    const enrollmentData = await fetchEnrollmentData(employerId, employeeId);
    fetchedEnrollments = enrollmentData.enrollments;
    fetchedPackages = enrollmentData.packages;
    fetchedEmployerId = enrollmentData.employerId;
    fetchedEmployeeId = enrollmentData.employeeId;
    allPlans = extractPackagesPlans(fetchedPackages);

    route = '/Enrollments';
    fetchRoute(route, onSuccess, null);
}

function closeEnrollments() {
    showEmployees(fetchedEmployerId);
}

function showEnrollmentModal(enrollmentId, planId) {
    var plan = getItemById(allPlans, planId);
    var validPlans = getValidPlans(plan);

    if (validPlans.length == 0) {
        showError('enrollments-error', "You don't have any valid plans");
        return;
    }

    hideError('enrollments-error');

    var modal = document.getElementById("enrollment-details-modal");
    modal.style.display = "block";

    var selectedPlanIndex = findItemArrayIndex(validPlans, planId);
    populatePlanOptions(validPlans, selectedPlanIndex);

    var submitButton = document.getElementById("enrollment-submit-btn");
    var electionsInput = document.getElementById("elections-input");
    var enrollment = getItemById(fetchedEnrollments, enrollmentId);

    if (enrollmentId != -1) {
        electionsInput.value = enrollment.election;

        submitButton.onclick = function () {
            updateEnrollment(enrollmentId);
        };
    } else {
        submitButton.onclick = addEnrollment;
    }
}

function closeEnrollmentModal() {
    var modal = document.getElementById("enrollment-details-modal");
    modal.style.display = "none";
    hideError('modal-error');
}

async function fetchEnrollmentData(employerId, employeeId) {
    try {
        const route = '/Enrollments/Data?employerId=' + employerId + '&employeeId=' + employeeId;

        const response = await fetch(route);

        if (!response.ok) {
            throw new Error("Response was not ok");
        }

        const data = await response.json();

        const enrollments = data.enrollments;
        const packages = data.packages;
        const fetchedEmployeeId = data.employeeId;
        const fetchedEmployerId = data.employerId;

        return {
            enrollments,
            packages,
            employeeId: fetchedEmployeeId,
            employerId: fetchedEmployerId
        };
    } catch (error) {
        console.error("Fetch request failed: " + error);
    }
}

function addEnrollment() {
    var enrollmentForm = document.getElementById('enrollment-form');
    var formData = new FormData(enrollmentForm);
    var election = formData.get('elections');

    if (election == '') {
        showError('modal-error', 'Election field cannot be empty');
        return;
    }

    if (election < 0) {
        showError('modal-error', 'Election cannot be less than 0');
        return;
    }

    let selectedPlan = getItemById(allPlans, formData.get('plan'));

    fetchedEnrollments.push({
        'contribution': formData.get('contributions'),
        'election': formData.get('elections'),
        'employeeId': fetchedEmployeeId,
        'plan': selectedPlan
    });

    closeEnrollmentModal();
    hideError('modal-error');
    handleEnrollmentsSection();
}

function updateEnrollment(enrollmentId) {
    var enrollmentIndex = findItemArrayIndex(fetchedEnrollments, enrollmentId);

    if (enrollmentIndex == -1) {
        console.error('Enrollment with specified id was not found');
        return;
    }

    var enrollmentForm = document.getElementById('enrollment-form');
    var formData = new FormData(enrollmentForm);
    var election = formData.get('elections');

    if (election == '') {
        showError('modal-error', 'Election field cannot be empty');
        return;
    }

    if (election < 0) {
        showError('modal-error', 'Election cannot be less than 0');
        return;
    }

    let selectedPlan = getItemById(allPlans, formData.get('plan'));

    fetchedEnrollments[enrollmentIndex]['election'] = formData.get('elections');
    fetchedEnrollments[enrollmentIndex]['plan'] = selectedPlan;
    fetchedEnrollments[enrollmentIndex]['contribution'] = selectedPlan.contribution;

    closeEnrollmentModal();
    hideError('modal-error');
    handleEnrollmentsSection();
}

function populatePlanOptions(plans, selectedPlanIndex) {
    var selectElement = document.getElementById("plan-options");
    selectElement.innerHTML = "";

    plans.forEach(function (plan) {
        let packageName = getItemById(fetchedPackages, plan.packageId).name;
        var option = document.createElement("option");
        option.value = plan.id;
        option.text = plan.name + ` (${packageName})`;
        selectElement.appendChild(option);
    });

    if (selectedPlanIndex != -1) {
        selectElement.selectedIndex = selectedPlanIndex;
    }

    var contributionsText = document.getElementById("contributions-text");
    contributionsText.innerHTML = plans[0].contribution;

    var hiddenContributionsInput = document.getElementById("contributions-input");
    hiddenContributionsInput.value = plans[0].contribution;
}

function onSelectedPlanChange() {
    var planOptions = document.getElementById('plan-options');
    var selectedPlanId = planOptions.value;
    var selectedPlan = getItemById(allPlans, selectedPlanId);

    var contributionsText = document.getElementById("contributions-text");
    contributionsText.innerText = selectedPlan.contribution;

    var hiddenContributionsInput = document.getElementById("contributions-input");
    hiddenContributionsInput.value = selectedPlan.contribution;
}

function handleEnrollmentsSection() {
    if (fetchedEnrollments.length == 0) {
        showEmptyEnrollmentsSection();
        return;
    }

    showEnrollmentsSection();
}

function showEnrollmentsSection() {
    var enrollmentsTableWrapper = document.getElementById("enrollments-table-wrapper");
    var noActiveEnrollmentsWrapper = document.getElementById("no-active-enrollments-wrapper")

    var enrollmentsTableBody = document.getElementById('enrollments-table-body');
    let html = '';

    fetchedEnrollments.forEach(function (enrollment) {
        let package = getItemById(fetchedPackages, enrollment.plan.packageId);
        html += '<tr>';
        html += '<td>' + enrollment.plan.name + ` (${package.name})` + '</td>';
        html += '<td>' + enrollment.election + '</td>';
        html += '<td>' + enrollment.contribution + '</td>';
        html += '<td>';
        if (!didPackageEnd(package)) {
            html += '<button class="btn btn-secondary update-button" ';
            html += `onclick = "showEnrollmentModal(${enrollment.id}, ${enrollment.plan.id}, ${enrollment.election})">`;
            html += 'Update';
            html += '</button></td>';
        }
        html += '</tr>';
        html += '</tr>';
    });

    enrollmentsTableBody.innerHTML = html;

    enrollmentsTableWrapper.style.display = "block";
    noActiveEnrollmentsWrapper.style.display = "none";
}

function showEmptyEnrollmentsSection() {
    var enrollmentsTableWrapper = document.getElementById("enrollments-table-wrapper");
    var noActiveEnrollmentsWrapper = document.getElementById("no-active-enrollments-wrapper")

    noActiveEnrollmentsWrapper.style.display = "block";
    enrollmentsTableWrapper.style.display = "none";
}

function getItemById(array, id) {
    return array.find(item => item.id == id);
}

function getUnrolledPlans(allPlans, enrolledPlans) {
    return allPlans.filter(plan => !enrolledPlans.some(otherPlan => otherPlan.id === plan.id));
}

function getActivePlans(allPlans) {
    activePlans = []
    for (let i = 0; i < allPlans.length; i++) {
        let package = getItemById(fetchedPackages, allPlans[i].packageId);
        if (package.isInitialized == true && !didPackageEnd(package)) {
            activePlans.push(allPlans[i]);
        }
    }
    return activePlans;
}

function didPackageEnd(package) {
    const endDate = new Date(package.planEnd);
    const endDateTimestamp = endDate.getTime();
    const currentUTCDate = new Date();
    const currentUTCTimestamp = currentUTCDate.getTime();
    return currentUTCTimestamp >= endDateTimestamp;
}

function extractPackagesPlans(packages) {
    return packages.reduce((accumulator, currentPackage) => {
        accumulator.push(...currentPackage.plans);
        return accumulator;
    }, []);
}

function getValidPlans(excludedPlan) {
    var enrolledPlans = extractEnrollmentsPlans(fetchedEnrollments);
    var unrolledPlans = getUnrolledPlans(allPlans, enrolledPlans);
    var activePlans = getActivePlans(unrolledPlans);

    if (excludedPlan != null) {
        activePlans.push(excludedPlan);
    }
    
    return activePlans;
}

function extractEnrollmentsPlans(enrollments) {
    let plans = [];

    enrollments.forEach(enrollment => {
        plans.push(enrollment.plan);
    });

    return plans;
}

function findItemArrayIndex(array, id) {
    for (let i = 0; i < array.length; i++) {
        if (array[i].id === id) {
            return i;
        }
    }
    return -1;
}

function submitEnrollmentChanges() {

    const filteredEnrollments = fetchedEnrollments.filter(enrollment => {
        const associatedPackage = fetchedPackages.find(package => package.packageId === enrollment.packageId);

        return associatedPackage && new Date(associatedPackage.EndDate) < new Date(Date.UTC());
    });

    const options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(filteredEnrollments)
    };

    fetch('/Enrollments/Update?employeeId=' + fetchedEmployeeId + '&employerId=' + fetchedEmployerId, options)
        .then(async response => {
            var json = await response.json();

            if (!response.ok) {
                showError('enrollments-error', json.detail);
                throw new Error(json.detail);
            }

            return json;
        })
        .then(data => {
            closeEnrollments();
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}