using Swashbuckle.AspNetCore.SwaggerGen;
using Umbraco.Cms.Api.Common.DependencyInjection;
using Umbraco.Cms.Core;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoBlazorHeadless.CMS.Controllers;
using UmbracoBlazorHeadless.CMS.Custom;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

builder.Services.AddCors(options => options.AddDefaultPolicy(builder => builder
    .WithOrigins("https://localhost:44369", "https://localhost:7139")
    .WithHeaders("Preview", "Api-Key")));

// Workaround for the fact that using System.Text.Json the type discriminator needs to be the first property!
builder.Services.AddControllers().AddJsonOptions(Constants.JsonOptionsNames.DeliveryApi, options =>
{
    options.JsonSerializerOptions.TypeInfoResolver = new CustomDeliveryApiJsonTypeResolver();
});

// Enable generation of typed content responses based on CMS content types
builder.Services.Configure<SwaggerGenOptions>(options =>
{
    options.SupportNonNullableReferenceTypes();

    // UseOneOfForPolymorphism is disabled as we are consuming the swagger with NSwag
    // If using other code generations tools, like Orval, it should be enabled for better compatibility
    //options.UseOneOfForPolymorphism();

    options.UseAllOfForInheritance();

    options.SchemaFilter<DeliveryApiContentTypesSchemaFilter>();
});

builder.Services.Configure<UmbracoRenderingDefaultsOptions>(c =>
{
    c.DefaultControllerType = typeof(CustomControllerRouteController);
});

WebApplication app = builder.Build();

await app.BootUmbracoAsync();

app.UseHttpsRedirection();

app.UseCors();
app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseInstallerEndpoints();
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });


await app.RunAsync();
