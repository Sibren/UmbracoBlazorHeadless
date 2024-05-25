using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Diagnostics;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace UmbracoBlazorHeadless.CMS.Controllers
{
    public class CustomControllerRouteController : RenderController
    {
        private readonly IConfiguration configuration;

        public CustomControllerRouteController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, IConfiguration configuration) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            this.configuration = configuration;
        }
        public override IActionResult Index()
        {
            // Here maybe ignore some AzureAd/Entra-urls, but that's out of scope
            if (!UmbracoContext.InPreviewMode || CurrentPage == null)
            {
                // Since this might be logged somewhere and you might not want that, you can also change this.
                // other option: redirect to Umbraco
                throw new ArgumentNullException("You're not able to see this page!");
            }
            var currentRoute = CurrentPage!.UrlSegment;
            var previewHost = configuration.GetValue<string>("FrontendUrl");
            if (previewHost == null)
            {
                throw new ArgumentNullException("No preview host in appsettings found!");
            }

            // it's out of scope for this simple demo, but you could here return to https://preview.yoursite.com which you host on Azure and has MS Entra in front of it and always uses the preview.
            return Redirect($"{previewHost.TrimEnd('/')}/{currentRoute}");
        }
    }
}
