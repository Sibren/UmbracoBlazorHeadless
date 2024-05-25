using UmbracoBlazorHeadless.Frontend.Extensions;

namespace UmbracoBlazorHeadless.Frontend.Umbraco;

public interface IUmbracoClient
{
    Task<PagedIApiContentResponseModel?> GetContentByPath(string path);

    Task<IApiContentResponseModel?> GetSingleContentByPath(string path);

    Task<T?> GetContentSingleByType<T>(CancellationToken cancellationToken = default) where T : IApiContentResponseModel;

    Task<ICollection<IApiContentResponseModel>> GetChildrenByPath(string path, string[]? filter = null, string[]? sort = null, CancellationToken cancellationToken = default);

    Task<(ICollection<IApiContentResponseModel> Pages, long Total)> Search(string query, int skip, int take, CancellationToken cancellationToken = default);
}

internal class UmbracoClient(IUmbracoApi umbracoApi, IConfiguration config) : IUmbracoClient
{
    private readonly string? _apiKey = config["UmbracoApi:ApiKey"];
    private readonly bool isInPreview = config.GetValue<bool>("UmbracoApi:IsInPreview");

    public async Task<PagedIApiContentResponseModel?> GetContentByPath(string path)
    {
        try
        {
            var response = await umbracoApi.GetContent2_0Async(path, preview: isInPreview, api_Key: isInPreview ? _apiKey : null);
            return response;
        }
        catch (ApiException e) when (e.StatusCode == 404)
        {
            return null;
        }
    }

    public async Task<IApiContentResponseModel?> GetSingleContentByPath(string path)
    {
        try
        {
            var response = await umbracoApi.GetContentItemByPath2_0Async(path, preview: isInPreview, api_Key: isInPreview ? _apiKey : null);
            return response;
        }
        catch (ApiException e) when (e.StatusCode == 404)
        {
            return null;
        }
    }

    public async Task<T?> GetContentSingleByType<T>(CancellationToken cancellationToken = default) where T : IApiContentResponseModel
    {
        try
        {
            var response = await umbracoApi.GetContent2_0Async(filter: new[] { $"contentType:{typeof(T).GetContentTypeAlias()}" }, take: 1, preview: isInPreview, api_Key: isInPreview ? _apiKey : null, cancellationToken: cancellationToken);
            return response.Items.FirstOrDefault() as T;
        }
        catch (ApiException e) when (e.StatusCode == 404)
        {
            return null;
        }
    }

    public async Task<ICollection<IApiContentResponseModel>> GetChildrenByPath(string path, string[]? filter = null, string[]? sort = null, CancellationToken cancellationToken = default)
    {
        var response = await umbracoApi.GetContent2_0Async(fetch: $"children:{path}", filter: filter ?? Enumerable.Empty<string>(), sort: sort ?? new[] { "sortOrder:asc" }, preview: isInPreview, api_Key: isInPreview ? _apiKey : null, cancellationToken: cancellationToken);
        return response.Items;
    }

    public async Task<(ICollection<IApiContentResponseModel> Pages, long Total)> Search(string query, int skip, int take, CancellationToken cancellationToken = default)
    {
        PagedIApiContentResponseModel response = await umbracoApi.GetContent2_0Async(filter: new[] { $"search:{query}", "hideFromSearch:false" }, skip: skip, take: take, preview: isInPreview, api_Key: isInPreview ? _apiKey : null, cancellationToken: cancellationToken);
        return (response.Items, response.Total);
    }
}