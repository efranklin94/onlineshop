namespace onlineshop.Proxies;

public interface ITrackingCodeProxy
{
    public Task<List<string>> Get(int count, CancellationToken cancellationToken);
}