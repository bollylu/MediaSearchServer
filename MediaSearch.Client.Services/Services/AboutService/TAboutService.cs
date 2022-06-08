﻿using BLTools.Text;

namespace MediaSearch.Client.Services;

public class TAboutService : IAboutService, ILoggable {

  public IApiServer ApiServer { get; set; } = new TApiServer();
  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TAboutService>();

  public TAboutService() { }

  public async Task<TAbout?> GetAboutAsync(string name = "server") {
    try {

      string RequestUrl = $"about?name={name.ToUrl()}";
      Logger.LogDebugEx(RequestUrl.BoxFixedWidth($"get about from server", GlobalSettings.DEBUG_BOX_WIDTH));

      using (CancellationTokenSource Timeout = new CancellationTokenSource(GlobalSettings.HTTP_TIMEOUT_IN_MS)) {

        TAbout? Result = await ApiServer.GetJsonAsync<TAbout>(RequestUrl, CancellationToken.None).ConfigureAwait(false);
        if (Result is null) {
          return null;
        }
        Logger.LogDebugExBox("TAbout", Result);
        return Result;

      }
    } catch (Exception ex) {
      Logger.LogError($"Unable to get about of server services : {ex.Message}");
      if (ex.InnerException is not null) {
        Logger.LogError($"  Inner exception : {ex.InnerException.Message}");
        Logger.LogError($"  Inner call stack : {ex.InnerException.StackTrace}");
      }
      return null;
    }
  }


}
