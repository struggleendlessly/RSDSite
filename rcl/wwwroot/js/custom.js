window.js_downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);

    const url = URL.createObjectURL(blob);

    triggerFileDownload(fileName, url);

    URL.revokeObjectURL(url);
}

function js_enablePopovers() {
    const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]')
    const popoverList = [...popoverTriggerList].map(popoverTriggerEl => new bootstrap.Popover(popoverTriggerEl))
}

function js_showAndHideAlert(alertId, alertClass, alertMessage) {
    var alertElement = document.getElementById(alertId);
    if (alertElement) {
        alertElement.innerText = alertMessage;
        alertElement.classList.remove('d-none');
        alertElement.classList.add(alertClass);
        alertElement.classList.add('d-block');

        setTimeout(function () {
            alertElement.classList.remove('d-block');
            alertElement.classList.remove(alertClass);
            alertElement.classList.add('d-none');
            alertElement.innerText = '';
            
        }, 3000);
    }
}

function js_editorActivate(id) {
    var selector = `#${id}`;

    const toolbarOptions = [
        [{ 'font': [] }, { header: [1, 2, 3, false] }],
        ['size', 'bold', 'italic', 'underline', 'strike'],
        [{ 'color': [] }, { 'background': [] }],
        [{ 'script': 'sub' }, { 'script': 'super' }],
        [{ 'header': 1 }, { 'header': 2 }, 'blockquote', 'code-block'], 
        [{ 'list': 'ordered' }, { 'list': 'bullet' }, { 'indent': '-1' }, { 'indent': '+1' }],
        [{ 'direction': 'rtl' }, { 'align': [] }],
        ['link', 'image', 'video', 'formula'],
        ['clean']
    ];

    const quill = new Quill(selector, {
        modules: {
            toolbar: toolbarOptions
        },
        theme: 'snow'
    });
}

async function js_editorGetContent(id, format) {
    var quillContainer = document.getElementById(id);
    var childElement = quillContainer.querySelector('.ql-editor');
    var content = childElement.innerHTML;

    // Check if content starts with <p> and ends with </p>
    if (content.startsWith('<p>') && content.endsWith('</p>')) {
        content = content.substring(3, content.length - 4);
    }

    // Find and replace base64 images
    var base64Images = content.match(/<img[^>]+src="data:image\/(.*?);base64,([^"]+)"[^>]*>/g);
    if (base64Images) {
        for (let i = 0; i < base64Images.length; i++) {
            var base64Data = base64Images[i].match(/data:image\/(.*?);base64,([^"]+)/)[2];

            var resizedBase64 = await scaleImageToFullHD(base64Data);
            var azureBlobLink = await DotNetHelpers.uploadImage(resizedBase64);

            content = content.replace(base64Images[i], `<img src="${azureBlobLink}" alt="Uploaded Image">`);
        }
    }

    return content;
}

async function js_imageEditorGetContent(id) {
    return new Promise(async (resolve, reject) => {
        var fileInput = document.getElementById(id);
        if (fileInput.files.length > 0) {
            const file = fileInput.files[0];
            const reader = new FileReader();

            reader.onload = async function (event) {
                const base64Image = event.target.result;
                var base64Data = base64Image.match(/data:image\/(.*?);base64,([^"]+)/)[2];

                var resizedBase64 = await scaleImageToFullHD(base64Data);
                var azureBlobLink = await DotNetHelpers.uploadImage(resizedBase64);

                fileInput.value = '';

                resolve(azureBlobLink);
            };

            reader.onerror = function (error) {
                console.error('Error reading file: ', error);
                alert('Error reading file: ' + error);
                resolve(null);
            };

            reader.readAsDataURL(file);
        } else {
            console.error('No file selected');
            alert('No file selected');
            resolve(null);
        }
    });
}

function js_leafletActivate(accessToken, coordinates, markerText) {

    var coordinatesParts = coordinates.split(',');
    let latitude = coordinatesParts[0].trim();
    let longitude = coordinatesParts[1].trim();

    var map = L.map('map').setView([latitude, longitude], 13);

    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=' + accessToken, {
        id: 'mapbox/light-v9'
    }).addTo(map);

    var marker = L.marker([latitude, longitude]).addTo(map);

    marker.bindPopup(markerText);
}

async function scaleImageToFullHD(base64Image) {
    return new Promise((resolve, reject) => {
        const img = new Image();
        const dataUriScheme = "data:image/png;base64,";

        img.src = dataUriScheme + base64Image;

        img.onload = async function () {
            const maxWidth = 1920;
            const maxHeight = 1080;
            let width = img.width;
            let height = img.height;

            if (width > maxWidth || height > maxHeight) {
                if (width / height > maxWidth / maxHeight) {
                    if (width > maxWidth) {
                        height *= maxWidth / width;
                        width = maxWidth;
                    }
                } else {
                    if (height > maxHeight) {
                        width *= maxHeight / height;
                        height = maxHeight;
                    }
                }
            }

            const canvas = document.createElement('canvas');
            canvas.width = width;
            canvas.height = height;

            const ctx = canvas.getContext('2d');
            ctx.drawImage(img, 0, 0, width, height);

            const resizedBase64 = canvas.toDataURL('image/png');
            const resizedBase64WithoutDataUriScheme = resizedBase64.replace(dataUriScheme, '');

            resolve(resizedBase64WithoutDataUriScheme);
        };

        img.onerror = async function (error) {
            reject(error);
        };
    });
}
