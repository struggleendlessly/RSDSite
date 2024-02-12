using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using shared;
using shared.Managers;
using System.Net.Http.Json;

namespace web.Client.Pages
{
    public partial class AboutUsOld : IDisposable
    {
        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        NavigationManager navigationManager { get; set; }

        public AboutUsPageModel Model { get; set; } = new AboutUsPageModel();
        public string ShowTinyMCE { get; set; } = StaticHtmlStrings.CSSDisplayNone;
        public string BtnEdit { get; set; } = StaticHtmlStrings.CSSDisplayInline;
        public string BtnSave { get; set; } = StaticHtmlStrings.CSSDisplayNone;
        public string BtnCancel { get; set; } = StaticHtmlStrings.CSSDisplayNone;

        protected override async Task OnInitializedAsync()
        {
            var jsonFilePath = new Uri(new Uri(navigationManager.BaseUri), StaticStrings.AboutUsPageDataJsonFilePath);
            Model = await new HttpClient().GetFromJsonAsync<AboutUsPageModel>("https://localhost:7101/about-us.json");
        }

        private DotNetObjectReference<AboutUsOld>? dotNetHelper;

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
            await JS.InvokeVoidAsync(JSInvokeMethodList.tinymceActivate, StaticHtmlStrings.TinyAboutUsTitleId);
        }

        protected async void TinyMceCancel()
        {
            BtnEdit = StaticHtmlStrings.CSSDisplayInline;
            BtnSave = StaticHtmlStrings.CSSDisplayNone;
            BtnCancel = StaticHtmlStrings.CSSDisplayNone;

            ShowTinyMCE = StaticHtmlStrings.CSSDisplayNone;
            //await JS.InvokeVoidAsync("js_tinymceActivate");
        }

        protected async void TinyMceSave(string propertyName)
        {
            BtnEdit = StaticHtmlStrings.CSSDisplayInline;
            BtnSave = StaticHtmlStrings.CSSDisplayNone;
            BtnCancel = StaticHtmlStrings.CSSDisplayNone;

            ShowTinyMCE = StaticHtmlStrings.CSSDisplayNone;

            //await JS.InvokeVoidAsync(JSInvokeMethodList.tinymceGetContent);
            var content = await JS.InvokeAsync<string>(JSInvokeMethodList.tinymceGetContent, StaticHtmlStrings.TinyAboutUsTitleId);
            var property = typeof(AboutUsPageModel).GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(Model, content);
            }

            //JsonFileManager.WriteToJsonFile(Model, StaticStrings.AboutUsPageDataJsonFilePath);
            StateHasChanged();
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

    public class AboutUsPageModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
    }
}
