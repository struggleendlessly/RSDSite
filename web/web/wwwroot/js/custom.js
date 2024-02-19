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
        selector: selector
    });
}

function js_tinymceDestroy(id) {
    tinymce.get(id).remove();
}

function js_tinymceGetContent(id, format) {
    return tinymce.get(id).getContent({ format: format });
}