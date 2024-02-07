window.js_downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);

    const url = URL.createObjectURL(blob);

    triggerFileDownload(fileName, url);

    URL.revokeObjectURL(url);
}

function js_tinymceActivate() {
    tinymce.init({
        selector: 'textarea#tiny'
    });
}
function js_tinymceGetContent() {
    var myContent = tinymce.activeEditor.getContent();
    DotNetHelpers.sayHello(myContent);
}

