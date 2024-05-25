using Microsoft.AspNetCore.Components;
using UmbracoBlazorHeadless.Frontend.Extensions;
using UmbracoBlazorHeadless.Frontend.Umbraco;

namespace UmbracoBlazorHeadless.Frontend.Shared
{
    public partial class NavMenu : ComponentBase
    {
        [Inject]
        public required IUmbracoClient UmbracoClient { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await LoadNavigation();
        }

        private bool IsLoaded = true;
        IEnumerable<IApiContentResponseModel> NavigationPages { get; set; }

        private async Task LoadNavigation(bool forceUpdate = false)
        {
            IsLoaded = false;

            var slug = $"/";
            var childItems = await UmbracoClient.GetChildrenByPath(slug);
            NavigationPages = childItems;//.Where(x => x.Properties.BoolIsTrue("hideFromMenu"));

            IsLoaded = true;
        }
    }
}
