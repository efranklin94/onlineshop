using Microsoft.Extensions.Options;

namespace onlineshop.Proxies;

public class TrackingCodeProxy(IOptions<Settings> options) : ITrackingCodeProxy
{
    private readonly TrackingCodeSettings _settings = options.Value.TrackingCode;

    public async Task<string> Get(CancellationToken cancellationToken)
    {
        var httpClient = new HttpClient()
        {
            BaseAddress = new Uri(_settings.BaseURL)
        };

        var url = string.Format(_settings.GetURL, _settings.Prefix);

        using HttpResponseMessage response = await httpClient.GetAsync(url, cancellationToken);

        response.EnsureSuccessStatusCode();

        var trackingCode = await response.Content.ReadAsStringAsync(cancellationToken);
        return trackingCode;
    }
}
