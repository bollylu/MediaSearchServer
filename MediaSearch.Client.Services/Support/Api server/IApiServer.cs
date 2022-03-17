namespace MediaSearch.Client.Services;

public interface IApiServer  {

  Uri BaseAddress { get; }
  int RequestId { get; }

  Task<string?> GetStringAsync(string uriRequest, CancellationToken cancellationToken);
  Task<string?> GetStringAsync<T>(string uriRequest, T additionalContent, CancellationToken cancellationToken);
  //Task<string?> GetStringAsync(string uriRequest, IJson additionalContent, CancellationToken cancellationToken);
  Task<T?> GetJsonAsync<T>(string uriRequest, CancellationToken cancellationToken) where T : class, IJson<T>;
  Task<T?> GetJsonAsync<C, T>(string uriRequest, IJson<C> additionalContent, CancellationToken cancellationToken) where T : class, IJson<T> where C : class, IJson<C> ;

  Task<byte[]?> GetByteArrayAsync(string uriRequest, CancellationToken cancellationToken);
  Task<Stream> GetStreamAsync(string uriRequest, CancellationToken cancellationToken);

  Task<bool> ProbeServerAsync(CancellationToken cancellationToken);

}
