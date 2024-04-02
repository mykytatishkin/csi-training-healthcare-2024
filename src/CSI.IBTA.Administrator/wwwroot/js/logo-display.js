function getEncodedLogo(encodedLogo) {
    if (encodedLogo != null && encodedLogo != "") {
        try {
            window.atob(encodedLogo);
            document.getElementById('logo-employer').src = "data:image/png;base64," + encodedLogo;
            return;
        } catch (e) { }
    }
    hideLogo();
}

function hideLogo() {
    document.getElementById('logo-employer').src = "";
}

function showLogo() {
    document.getElementById('logo-employer').hidden = false;
}