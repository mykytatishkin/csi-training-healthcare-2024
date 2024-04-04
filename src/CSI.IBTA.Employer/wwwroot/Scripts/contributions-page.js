function showContributions() {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;

        document.getElementById('contributions-file-input').addEventListener('change', function () {
            if (this.files.length == 0) {
                document.querySelector('.file-name').textContent = 'No file selected';
                return;
            }
            var fileName = this.files[0].name;
            document.querySelector('.file-name').textContent = fileName;
        });
    }

    route = '/Contributions';
    fetchRoute(route, onSuccess, null);
}

function importContributions() {
    let successElement = document.getElementById('import-contributions-success');
    successElement.style.display = 'none';
    let errorsElement = document.getElementById('import-contributions-errors');
    errorsElement.innerText = '';

    const form = document.getElementById('import-contributions-form');
    const formData = new FormData(form);

    const fileInput = document.getElementById('contributions-file-input');

    if (fileInput.files.length == 0) {
        let errorsElement = document.getElementById('import-contributions-errors');
        errorsElement.innerText = 'Select contributions file';
        return;
    }

    formData.append('file', fileInput.files[0]);

    const options = {
        method: 'POST',
        body: formData
    };

    fetch('/Contributions/Upload', options)
        .then(async response => {
            if (!response.ok) {
                let responseJson = await response.json();
                if (responseJson.errors == null) {
                    throw new Error('Network response was not ok');
                } else {
                    let errorsElement = document.getElementById('import-contributions-errors');
                    errorsElement.innerText = '';
                    errorsElement.innerText += 'The import file does not pass validation. \n';
                    responseJson.errors.forEach(error => {
                        errorsElement.innerText += error + '\n';
                    });
                }
            } else {
                successElement.style.display = 'block';
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}
