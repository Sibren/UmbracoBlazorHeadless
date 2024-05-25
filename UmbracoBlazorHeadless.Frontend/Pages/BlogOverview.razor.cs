using Microsoft.AspNetCore.Components;

namespace UmbracoBlazorHeadless.Frontend.Pages
{
    public partial class BlogOverview : ComponentBase
    {
        [Parameter]
        public BlogOverviewContentModel? Page { get; set; }

        protected override async Task OnParametersSetAsync()
        {

        }
    }
}
