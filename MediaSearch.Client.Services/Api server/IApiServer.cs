namespace MediaSearch.Client.Services;

public interface IApiServer {

  Task<string?> GetStringAsync(string uriRequest, CancellationToken cancellationToken);
  Task<string?> GetStringAsync(string uriRequest, string additionalContent, CancellationToken cancellationToken);
  Task<string?> GetStringAsync(string uriRequest, IJson additionalContent, CancellationToken cancellationToken);
  Task<T?> GetJsonAsync<T>(string uriRequest, CancellationToken cancellationToken) where T : IJson<T>;
  Task<T?> GetJsonAsync<T>(string uriRequest, IJson additionalContent, CancellationToken cancellationToken) where T : IJson<T>;

  Task<bool> ProbeServerAsync(CancellationToken cancellationToken);

}
