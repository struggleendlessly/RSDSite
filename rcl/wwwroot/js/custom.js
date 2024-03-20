window.js_downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);

    const url = URL.createObjectURL(blob);

    triggerFileDownload(fileName, url);

    URL.revokeObjectURL(url);
}

function js_tinymceActivate(id) {
    var selector = `textarea#${id}`;

    tinymce.init({
        selector: selector,
        images_upload_handler: js_tinymceImageUploadHandler,
        paste_data_images: true,
        block_unsupported_drop: false
    });
}

const js_tinymceImageUploadHandler = (blobInfo, progress) => new Promise((resolve, reject) => {
    try {
        const blobInfoBase64 = blobInfo.base64();

        scaleImageToFullHD(blobInfoBase64)
            .then(resizedBase64 => {

                DotNetHelpers.uploadImage(resizedBase64).then(result => {
                    resolve(result);
                }).catch(error => {
                    console.log(error);
                    reject(error);
                });

            })
            .catch(error => {
                console.error('Error scaling image: ', error);
            });
    } catch (error) {
        console.error('Error: ', error);
        reject(error);
    }
});

function js_tinymceDestroy(id) {
    tinymce.get(id).remove();
}

function js_tinymceGetContent(id, format) {
    return tinymce.get(id).getContent({ format: format });
}

function js_leafletActivate(accessToken) {
    const leaflet = HSCore.components.HSLeaflet.init(document.getElementById('map'));

    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=' + accessToken, {
        id: 'mapbox/light-v9'
    }).addTo(leaflet);
}

function scaleImageToFullHD(base64Image) {
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