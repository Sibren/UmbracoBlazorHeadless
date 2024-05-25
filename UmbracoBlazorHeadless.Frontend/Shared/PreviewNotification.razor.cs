using Microsoft.AspNetCore.Components;

namespace UmbracoBlazorHeadless.Frontend.Shared
{
    public partial class PreviewNotification : ComponentBase
    {
        [Inject]
        public required IConfiguration Configuration { get; set; }

        private bool ShowPreviewNotification { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            ShowPreviewNotification = Configuration.GetValue<bool>("UmbracoApi:IsInPreview");
        }
    }
}
