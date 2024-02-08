class DotNetHelpers {
    static dotNetHelper;

    static setDotNetHelper(value) {
        DotNetHelpers.dotNetHelper = value;
    }

    static async sayHello(val) {
        const msg =
            await DotNetHelpers.dotNetHelper.invokeMethodAsync('returnTinyMceContent', val);
        //alert(`Message from .NET: "${msg}"`);
    }
}

window.DotNetHelpers = DotNetHelpers;