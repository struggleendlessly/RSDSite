using Microsoft.AspNetCore.Components;

namespace rcl
{
    public partial class SaveDataToJson
    {
        [Parameter]
        public string JsonData { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            base.OnParametersSet();
            // Add your code here
            File.WriteAllText("data.json", JsonData);
        }
    }
}
