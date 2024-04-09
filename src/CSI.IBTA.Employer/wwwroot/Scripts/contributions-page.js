function showContributions() {
    function onSuccess(data) {
        document.getElementById('main-partial-screen').innerHTML = data;
        addInputListener();
    }

    route = '/Contributions';
    fetchRoute(route, onSuccess, null);
}

function addInputListener() {
    document.getElementById('contributions-file-input').addEventListener('change', function () {
        if (this.files.length == 0) {
            document.querySelector('.file-name').textContent = 'No file selected';
            return;
        }
        var fileName = this.files[0].name;
        document.querySelector('.file-name').textContent = fileName;
    });
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
        errorsElement.innerText = 'Contributions file is not selected';
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
                if (response.status == 422) {
                    errorsElement.innerText = 'File is empty';
                    return;
                }

                let responseJson = await response.json();

                if (responseJson.errors == null) {
                    errorsElement.innerText = 'Something went wrong...';
                    throw new Error('Network response was not ok');
                }

                errorsElement.innerText = '';
                errorsElement.innerText += 'The import file does not pass validation. \n';
                responseJson.errors.forEach(error => {
                    errorsElement.innerText += error + '\n';
                });
            } else {
                function onSuccess(data) {
                    document.getElementById('main-partial-screen').innerHTML = data;
                    document.getElementById('import-contributions-success').style.display = 'block';
                    addInputListener();
                }

                route = '/Contributions';
                fetchRoute(route, onSuccess, null);
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}
