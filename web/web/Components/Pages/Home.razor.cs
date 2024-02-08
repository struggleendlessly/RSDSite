using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

using shared;

namespace web.Components.Pages
{
    public partial class Home : IDisposable
    {
        [Inject]
        IJSRuntime JS { get; set; }

        public Model Model { get; set; } = new Model();
        public string ShowTinyMCE { get; set; } = StaticHtmlStrings.CSSDisplayNone;
        public string BtnEdit { get; set; } = StaticHtmlStrings.CSSDisplayInline;
        public string BtnSave { get; set; } = StaticHtmlStrings.CSSDisplayNone;
        public string BtnCancel { get; set; } = StaticHtmlStrings.CSSDisplayNone;

        protected override async Task OnInitializedAsync()
        {

            Model = new Model
            {
                Title = ""
            };
        }

        private DotNetObjectReference<Home>? dotNetHelper;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                dotNetHelper = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync(JSInvokeMethodList.dotNetHelpersSetDotNetHelper, dotNetHelper);
            }
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }

        [JSInvokable]
        public void returnTinyMceContent(string content)
        {
            Model.Title = content;
        }

        protected async void TinyMceActivate()
        {
            BtnEdit = StaticHtmlStrings.CSSDisplayNone;
            BtnSave = StaticHtmlStrings.CSSDisplayInline;
            BtnCancel = StaticHtmlStrings.CSSDisplayInline;

            ShowTinyMCE = StaticHtmlStrings.CSSDisplayBlock;
            await JS.InvokeVoidAsync(JSInvokeMethodList.tinymceActivate);
        }

        protected async void TinyMceCancel()
        {
            BtnEdit = StaticHtmlStrings.CSSDisplayInline;
            BtnSave = StaticHtmlStrings.CSSDisplayNone;
            BtnCancel = StaticHtmlStrings.CSSDisplayNone;

            ShowTinyMCE = StaticHtmlStrings.CSSDisplayNone;
            //await JS.InvokeVoidAsync("js_tinymceActivate");
        }

        protected async void TinyMceSave()
        {
            BtnEdit = StaticHtmlStrings.CSSDisplayInline;
            BtnSave = StaticHtmlStrings.CSSDisplayNone;
            BtnCancel = StaticHtmlStrings.CSSDisplayNone;

            ShowTinyMCE = StaticHtmlStrings.CSSDisplayNone;

            await JS.InvokeVoidAsync(JSInvokeMethodList.tinymceGetContent);
        }

        private void Submit()
        {
            //Logger.LogInformation("Id = {Id} Description = {Description} " +
            //    "Classification = {Classification} MaximumAccommodation = " +
            //    "{MaximumAccommodation} IsValidatedDesign = " +
            //    "{IsValidatedDesign} ProductionDate = {ProductionDate}",
            //    Model?.Id, Model?.Description, Model?.Classification,
            //    Model?.MaximumAccommodation, Model?.IsValidatedDesign,
            //    Model?.ProductionDate);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            // Add your code here      
        }
    }

    public class Model
    {
        public string Title { get; set; }
    }
}
