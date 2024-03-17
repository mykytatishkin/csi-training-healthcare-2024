function getEncodedLogo(encodedLogo) {
    document.getElementById('logo-employer').src = "data:image/png;base64," + encodedLogo;
    showLogo();
}

function hideLogo() {
    console.log("hideLogo()")
    document.getElementById('logo-employer').hidden = true;
}

function showLogo() {
    console.log("showLogo()")
    document.getElementById('logo-employer').hidden = false;
}