using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MediaSearch.Client.Services;

public class TApiServer : ALoggable, IApiServer {

  private readonly HttpClient _HttpClient;
  public HttpResponseMessage? LastResponse { get; private set; }

  public Uri BaseAddress => _HttpClient?.BaseAddress ?? new Uri("http://localhost");

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TApiServer() {
    _HttpClient = new HttpClient();
    SetLogger(GlobalSettings.GlobalLogger);
  }

  public TApiServer(Uri baseAddress) : this() {
    _HttpClient.BaseAddress = baseAddress;
  }

  public TApiServer(string baseAddress) : this() {
    _HttpClient.BaseAddress = new Uri(baseAddress);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.Append($"ApiServer : {BaseAddress.ToString().WithQuotes()}");
    return RetVal.ToString();
  }

  #region --- Get Json --------------------------------------------
  public async Task<T?> GetJsonAsync<T>(string uriRequest, CancellationToken cancellationToken) where T : IJson<T> {
    try {
      LogDebug($"Request: {uriRequest}");

      HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Get, uriRequest);
      RequestMessage.Headers.Add("Host", Environment.MachineName);

      LastResponse = await _HttpClient.SendAsync(RequestMessage, cancellationToken).ConfigureAwait(false);

      LogDebug($"Response: {LastResponse.StatusCode}");
      if (LastResponse.IsSuccessStatusCode) {
        string TextContent = await LastResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return T.FromJson(TextContent);
      } else {
        return default;
      }
    } catch (Exception ex) {
      Logger?.LogError($"Unable to read string from client : {ex.Message}");
      if (ex.InnerException is not null) {
        LogError($"  Inner exception : {ex.InnerException.Message}");
      }

      LastResponse = new HttpResponseMessage(HttpStatusCode.RequestTimeout);

      LogError($"{(int)LastResponse.StatusCode} : {LastResponse.ReasonPhrase}");
      return default;
    }
  }

  public async Task<T?> GetJsonAsync<T>(string uriRequest, IJson additionalContent, CancellationToken cancellationToken) where T : IJson<T> {
    try {
      LogDebug($"Request: {uriRequest}");

      HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Post, uriRequest);

      RequestMessage.Headers.Add("Host", Environment.MachineName);
      RequestMessage.Content = JsonContent.Create(additionalContent, MediaTypeHeaderValue.Parse("application/json"), TFilter.DefaultJsonSerializerOptions);

      LastResponse = await _HttpClient.SendAsync(RequestMessage, cancellationToken).ConfigureAwait(false);

      LogDebug($"Response: {LastResponse.StatusCode}");
      if (LastResponse.IsSuccessStatusCode) {
        string TextContent = await LastResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return T.FromJson(TextContent);
      } else {
        return default;
      }
    } catch (Exception ex) {
      Logger?.LogError($"Unable to read string from client : {ex.Message}");
      if (ex.InnerException is not null) {
        LogError($"  Inner exception : {ex.InnerException.Message}");
      }

      LastResponse = new HttpResponseMessage(HttpStatusCode.RequestTimeout);

      LogError($"{(int)LastResponse.StatusCode} : {LastResponse.ReasonPhrase}");
      return default;
    }
  }
  #endregion --- Get Json --------------------------------------------

  #region --- Get string --------------------------------------------
  public async Task<string?> GetStringAsync(string uriRequest, CancellationToken cancellationToken) {
    try {
      LogDebug($"Request: {uriRequest}");

      HttpRequestMessage RequestMessage = new(HttpMethod.Get, uriRequest);
      RequestMessage.Headers.Add("Host", Environment.MachineName);

      LastResponse = await _HttpClient.GetAsync(uriRequest, cancellationToken).ConfigureAwait(false);

      LogDebug($"Response: {LastResponse.StatusCode}");
      if (LastResponse.IsSuccessStatusCode) {
        return await LastResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
      } else {
        return null;
      }
    } catch (Exception ex) {
      LogError($"Unable to read string from client : {ex.Message}");
      if (ex.InnerException is not null) {
        Logger?.LogError($"  Inner exception : {ex.InnerException.Message}");
      }

      LastResponse = new HttpResponseMessage(HttpStatusCode.RequestTimeout);

      LogError($"{(int)LastResponse.StatusCode} : {LastResponse.ReasonPhrase}");
      return null;
    }
  }

  public async Task<string?> GetStringAsync(string uriRequest, string additionalContent, CancellationToken cancellationToken) {
    try {
      LogDebug($"Request: {uriRequest}");

      HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Post, uriRequest);
      RequestMessage.Headers.Add("Host", Environment.MachineName);
      RequestMessage.Content = new StringContent(additionalContent);

      LastResponse = await _HttpClient.SendAsync(RequestMessage, cancellationToken).ConfigureAwait(false);

      LogDebug($"Response: {LastResponse.StatusCode}");
      if (LastResponse.IsSuccessStatusCode) {
        return await LastResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
      } else {
        return null;
      }
    } catch (Exception ex) {
      LogError($"Unable to read string from client : {ex.Message}");
      if (ex.InnerException is not null) {
        Logger?.LogError($"  Inner exception : {ex.InnerException.Message}");
      }

      LastResponse = new HttpResponseMessage(HttpStatusCode.RequestTimeout);

      LogError($"{(int)LastResponse.StatusCode} : {LastResponse.ReasonPhrase}");
      return null;
    }
  }

  public async Task<string?> GetStringAsync(string uriRequest, IJson additionalContent, CancellationToken cancellationToken) {
    try {
      LogDebug($"Request: {uriRequest}");

      HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Post, uriRequest);
      RequestMessage.Headers.Add("Host", Environment.MachineName);
      RequestMessage.Content = JsonContent.Create(additionalContent);

      LastResponse = await _HttpClient.SendAsync(RequestMessage, cancellationToken).ConfigureAwait(false);

      LogDebugEx($"ResponseCode: {LastResponse.StatusCode}");
      LogDebugEx($"ResponseContent: {await LastResponse.Content.ReadAsStringAsync().ConfigureAwait(false)}");
      if (LastResponse.IsSuccessStatusCode) {
        return await LastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
      } else {
        return null;
      }
    } catch (Exception ex) {
      LogError($"Unable to read string from client : {ex.Message}");
      if (ex.InnerException is not null) {
        Logger?.LogError($"  Inner exception : {ex.InnerException.Message}");
      }

      LastResponse = new HttpResponseMessage(HttpStatusCode.RequestTimeout);

      LogError($"{(int)LastResponse.StatusCode} : {LastResponse.ReasonPhrase}");
      return null;
    }
  }
  #endregion --- Get string --------------------------------------------

  #region --- Probe --------------------------------------------
  public async Task<bool> ProbeServerAsync(CancellationToken cancellationToken) {

    try {

      HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Get, "about?name=server");
      LogDebug($"Probing server at {BaseAddress}");

      RequestMessage.Headers.Add("X-Client", Environment.MachineName);

      LastResponse = await _HttpClient.SendAsync(RequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

      LogDebugEx($"ResponseCode: {LastResponse.StatusCode}");
      LogDebugEx($"ResponseContent: {await LastResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)}");
      return LastResponse.IsSuccessStatusCode;
    } catch (Exception ex) {
      LogError($"Unable to probe server : {ex.Message}");
      return false;
    }

  }
  #endregion --- Probe --------------------------------------------

  public async Task<Stream> GetStreamAsync(string uriRequest, CancellationToken cancellationToken) {
    try {
      LogDebug($"Request: {uriRequest}");

      HttpRequestMessage RequestMessage = new(HttpMethod.Get, uriRequest);
      RequestMessage.Headers.Add("Host", Environment.MachineName);

      LastResponse = await _HttpClient.GetAsync(uriRequest, cancellationToken).ConfigureAwait(false);

      LogDebug($"Response: {LastResponse.StatusCode}");
      if (!LastResponse.IsSuccessStatusCode) {
        throw new HttpRequestException($"Error loading {uriRequest} : {LastResponse.StatusCode} {LastResponse.ReasonPhrase}");
      }

      return await LastResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

    } catch (Exception ex) {
      LogError($"Unable to read stream from client : {ex.Message}");
      if (ex.InnerException is not null) {
        Logger?.LogError($"  Inner exception : {ex.InnerException.Message}");
      }

      LastResponse = new HttpResponseMessage(HttpStatusCode.RequestTimeout);

      LogError($"{(int)LastResponse.StatusCode} : {LastResponse.ReasonPhrase}");
      throw;
    }
  }

  public async Task<byte[]?> GetByteArrayAsync(string uriRequest, CancellationToken cancellationToken) {
    try {
      LogDebug($"Request: {uriRequest}");

      HttpRequestMessage RequestMessage = new(HttpMethod.Get, uriRequest);
      RequestMessage.Headers.Add("Host", Environment.MachineName);

      LastResponse = await _HttpClient.GetAsync(uriRequest, cancellationToken).ConfigureAwait(false);

      LogDebug($"Response: {LastResponse.StatusCode}");
      if (!LastResponse.IsSuccessStatusCode) {
        throw new HttpRequestException($"Error loading {uriRequest} : {LastResponse.StatusCode} {LastResponse.ReasonPhrase}");
      }

      return await LastResponse.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);

    } catch (Exception ex) {
      LogError($"Unable to read stream from client : {ex.Message}");
      if (ex.InnerException is not null) {
        Logger?.LogError($"  Inner exception : {ex.InnerException.Message}");
      }

      LastResponse = new HttpResponseMessage(HttpStatusCode.RequestTimeout);

      LogError($"{(int)LastResponse.StatusCode} : {LastResponse.ReasonPhrase}");
      throw;
    }
  }

}
