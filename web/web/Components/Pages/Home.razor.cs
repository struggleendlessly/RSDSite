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
        public string ShowTinyMCE { get; set; } = "inline";
        public string BtnEdit { get; set; } = "inline";
        public string BtnSave { get; set; } = "none";
        public string BtnCancel { get; set; } = "none";

        protected override async Task OnInitializedAsync()
        {

            dotNetHelper = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync("DotNetHelpers.setDotNetHelper", dotNetHelper);

            Model = new Model
            {
                Title = ""
            };
        }

        private DotNetObjectReference<Home>? dotNetHelper;

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
            BtnEdit = "none";
            BtnSave = "inline";
            BtnCancel = "inline";

            ShowTinyMCE = "block";
            await JS.InvokeVoidAsync("js_tinymceActivate");
        }
        protected async void TinyMceCancel()
        {
            BtnEdit = "inline";
            BtnSave = "none";
            BtnCancel = "none";

            ShowTinyMCE = "none";
            //await JS.InvokeVoidAsync("js_tinymceActivate");
        }
        protected async void TinyMceSave()
        {
            BtnEdit = "inline";
            BtnSave = "none";
            BtnCancel = "none";

            ShowTinyMCE = "none";

            await JS.InvokeVoidAsync("js_tinymceGetContent");
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
