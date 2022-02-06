﻿using System.Net.Http.Json;

namespace MediaSearch.Client.Services;

public class TApiServer : ALoggable, IApiServer {

  private readonly HttpClient _HttpClient;
  public HttpResponseMessage? LastResponse { get; private set; }

  public Uri BaseAddress => _HttpClient?.BaseAddress ?? new Uri("http://localhost");

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TApiServer() {
    _HttpClient = new HttpClient();
    SetLogger(new TConsoleLogger());
    Logger.SeverityLimit = ESeverity.Debug;
  }

  public TApiServer(Uri baseAddress) : this() {
    _HttpClient.BaseAddress = baseAddress;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public async Task<T?> GetJsonAsync<T>(string uriRequest, CancellationToken cancellationToken) where T : IJson<T>, new() {
    try {
      LogDebug($"Request: {uriRequest}");

      HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Get, uriRequest);
      RequestMessage.Headers.Add("Host", Environment.MachineName);

      LastResponse = await _HttpClient.SendAsync(RequestMessage, cancellationToken).ConfigureAwait(false);

      LogDebug($"Response: {LastResponse.StatusCode}");
      if (LastResponse.IsSuccessStatusCode) {
        string TextContent = await LastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        T RetVal = new T();
        return RetVal.ParseJson(TextContent);
      } else {
        return default;
      }
    } catch (Exception ex) {
      Logger?.LogError($"Unable to read string from client : {ex.Message}");
      if (ex.InnerException is not null) {
        Logger?.LogError($"  Inner exception : {ex.InnerException.Message}");
      }

      LastResponse = new HttpResponseMessage(HttpStatusCode.RequestTimeout);

      LogError($"{(int)LastResponse.StatusCode} : {LastResponse.ReasonPhrase}");
      return default;
    }
  }

  public async Task<T?> GetJsonAsync<T>(string uriRequest, IJson additionalContent, CancellationToken cancellationToken) where T : IJson<T>, new() {
    try {
      LogDebug($"Request: {uriRequest}");

      HttpRequestMessage RequestMessage = new HttpRequestMessage(HttpMethod.Post, uriRequest);
      RequestMessage.Headers.Add("Host", Environment.MachineName);
      RequestMessage.Content = JsonContent.Create(additionalContent);

      LastResponse = await _HttpClient.SendAsync(RequestMessage, cancellationToken).ConfigureAwait(false);

      LogDebug($"Response: {LastResponse.StatusCode}");
      if (LastResponse.IsSuccessStatusCode) {
        string TextContent = await LastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        return T.FromJson(TextContent);
      } else {
        return default;
      }
    } catch (Exception ex) {
      Logger?.LogError($"Unable to read string from client : {ex.Message}");
      if (ex.InnerException is not null) {
        Logger?.LogError($"  Inner exception : {ex.InnerException.Message}");
      }

      LastResponse = new HttpResponseMessage(HttpStatusCode.RequestTimeout);

      LogError($"{(int)LastResponse.StatusCode} : {LastResponse.ReasonPhrase}");
      return default;
    }
  }

  public async Task<string?> GetStringAsync(string uriRequest, CancellationToken cancellationToken) {
    try {
      LogDebug($"Request: {uriRequest}");

      HttpRequestMessage RequestMessage = new(HttpMethod.Get, uriRequest);
      RequestMessage.Headers.Add("Host", Environment.MachineName);

      LastResponse = await _HttpClient.GetAsync(uriRequest, cancellationToken).ConfigureAwait(false);

      LogDebug($"Response: {LastResponse.StatusCode}");
      if (LastResponse.IsSuccessStatusCode) {
        return await LastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
      } else {
        return null;
      }
    } catch (Exception ex) {
      Logger?.LogError($"Unable to read string from client : {ex.Message}");
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
        return await LastResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
      } else {
        return null;
      }
    } catch (Exception ex) {
      Logger?.LogError($"Unable to read string from client : {ex.Message}");
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
      Logger?.LogError($"Unable to read string from client : {ex.Message}");
      if (ex.InnerException is not null) {
        Logger?.LogError($"  Inner exception : {ex.InnerException.Message}");
      }

      LastResponse = new HttpResponseMessage(HttpStatusCode.RequestTimeout);

      LogError($"{(int)LastResponse.StatusCode} : {LastResponse.ReasonPhrase}");
      return null;
    }
  }
}
