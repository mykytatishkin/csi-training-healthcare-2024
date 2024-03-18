function getEncodedLogo(encodedLogo) {
    document.getElementById('logo-employer').src = "data:image/png;base64," + encodedLogo;
}

function hideLogo() {
    document.getElementById('logo-employer').src = "";

}

function showLogo() {
    document.getElementById('logo-employer').hidden = false;
}