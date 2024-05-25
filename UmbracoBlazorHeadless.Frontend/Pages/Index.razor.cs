using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using UmbracoBlazorHeadless.Frontend.Extensions;
using UmbracoBlazorHeadless.Frontend.Umbraco;

namespace UmbracoBlazorHeadless.Frontend.Pages
{
    public partial class Index : ComponentBase
    {
        [Parameter]
        public required string Slug { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "query")]
        public string? Query { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await UpdatePage();
        }

        [Inject]
        public required IUmbracoClient UmbracoClient { get; set; }

        private bool isLoaded = true;
        private IApiContentResponseModel? pageData;
        private string metaDescription = string.Empty;

        private async Task UpdatePage(bool forceUpdate = false)
        {
            isLoaded = false;

            var slug = $"/{Slug}";
            if (slug != pageData?.Route.Path || forceUpdate)
            {
                pageData = await UmbracoClient.GetSingleContentByPath($"/{Slug}");
                //metaDescription = pageData.Properties.TryGetValue("metaDescription", out object? value) ? value.ToString() : "";
            }

            isLoaded = true;
        }

        private static readonly ConcurrentDictionary<string, Type?> Pages = new();
        private static Type? GetPageType(string contentTypeAlias) =>
            Pages.GetOrAdd(contentTypeAlias, key =>
            {
                var markerType = typeof(Index);
                return markerType.Assembly.GetType($"{markerType.Namespace}.{key}", false, true);
            });
    }
}
