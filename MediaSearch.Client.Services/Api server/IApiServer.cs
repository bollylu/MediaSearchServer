namespace MediaSearch.Client.Services;

internal interface IApiServer {

  Task<string> GetStringAsync(string uriRequest, CancellationToken cancellationToken);
  Task<string> GetStringAsync(string uriRequest, string additionalContent, CancellationToken cancellationToken);
  Task<string> GetStringAsync(string uriRequest, IJson additionalContent, CancellationToken cancellationToken);
  Task<T> GetJsonAsync<T>(string uriRequest, IJson additionalContent, CancellationToken cancellationToken) where T : IJson<T>, new();

}
