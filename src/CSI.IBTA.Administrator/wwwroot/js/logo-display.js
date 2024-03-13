function getEncodedLogo(encodedLogo) {
    document.getElementById('logo-employer').src = "data:image/png;base64," + encodedLogo;
    document.getElementById('logo-employer').hidden = false;
}

function hideLogo() {
    document.getElementById('logo-employer').hidden = true;

}

function showLogo() {
    document.getElementById('logo-employer').hidden = false;
}