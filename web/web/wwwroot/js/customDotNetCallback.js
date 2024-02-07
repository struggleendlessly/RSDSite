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

window.returnArrayAsync = (startPosition) => {
    DotNet.invokeMethodAsync('web', 'ReturnArrayAsync', startPosition)
        .then(data => {
            console.log(data);
        });
};

window.returnTinyMceContent = (content) => {
    DotNet.invokeMethodAsync('web', 'returnTinyMceContent', content)
        .then(data => {
            console.log(data);
        });
};

class DotNetHelpers {
    static dotNetHelper;

    static setDotNetHelper(value) {
        DotNetHelpers.dotNetHelper = value;
    }

    static async sayHello(val) {
        const msg =
            await DotNetHelpers.dotNetHelper.invokeMethodAsync('returnTinyMceContent', val);
        alert(`Message from .NET: "${msg}"`);
    }
}

window.DotNetHelpers = DotNetHelpers;