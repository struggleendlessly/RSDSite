window.js_downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);

    const url = URL.createObjectURL(blob);

    triggerFileDownload(fileName, url);

    URL.revokeObjectURL(url);
}

function js_editorActivate(id) {
    var selector = `#${id}`;

    const quill = new Quill(selector, {
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

function js_leafletActivate(accessToken) {
    const leaflet = HSCore.components.HSLeaflet.init(document.getElementById('map'));

    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=' + accessToken, {
        id: 'mapbox/light-v9'
    }).addTo(leaflet);
}

async function scaleImageToFullHD(base64Image) {
    return new Promise((resolve, reject) => {
        const img = new Image();
        const dataUriScheme = "data:image/png;base64,";

        img.src = dataUriScheme + base64Image;

        img.onload = function () {
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

        img.onerror = function (error) {
            reject(error);
        };
    });
}