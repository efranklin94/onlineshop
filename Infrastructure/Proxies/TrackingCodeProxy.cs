using Microsoft.Extensions.Options;
using System.Text.Json;
using TrackingCode.ViewModels;

namespace onlineshop.Proxies;

public class TrackingCodeProxy(IOptions<Settings> options) : ITrackingCodeProxy
{
    private readonly TrackingCodeSettings _settings = options.Value.TrackingCode;

    public async Task<List<string>> Get(int count, CancellationToken cancellationToken)
    {
        var httpClient = new HttpClient()
        {
            BaseAddress = new Uri(_settings.BaseURL)
        };

        var url = string.Format(_settings.GetURL, _settings.Prefix, count);

        using HttpResponseMessage response = await httpClient.GetAsync(url, cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync(cancellationToken);
        
        var resultJson =  JsonSerializer.Deserialize<GetTrackingCodesViewModel>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        
        return resultJson.TrackingCodes;
    }
}
