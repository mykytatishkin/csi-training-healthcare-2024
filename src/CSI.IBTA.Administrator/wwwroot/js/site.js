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

function showCreateEmployerForm() {
    $("#table-employer").hide();
    $("#control-employer-form").show();
}

function hideCreateEmployerForm() {
    $("#control-employer-form").hide();
    $("#table-employer").show();
}

function a(id) {
    alert(id)
}