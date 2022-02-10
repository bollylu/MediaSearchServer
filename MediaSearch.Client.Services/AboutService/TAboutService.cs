using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLTools.Text;

namespace MediaSearch.Client.Services;

public class TAboutService : ALoggable, IAboutService {

  public IApiServer ApiServer { get; set; } = new TApiServer();

  public TAboutService() {
    SetLogger(GlobalSettings.GlobalLogger);
  }

  public async Task<TAbout?> GetAboutAsync(string name = "server") {
    try {

      string RequestUrl = $"about?name={name.ToUrl()}";
      LogDebugEx(RequestUrl.BoxFixedWidth($"get about from server", GlobalSettings.DEBUG_BOX_WIDTH));

      using (CancellationTokenSource Timeout = new CancellationTokenSource(GlobalSettings.HTTP_TIMEOUT_IN_MS)) {

        TAbout? Result = await ApiServer.GetJsonAsync<TAbout>(RequestUrl, CancellationToken.None).ConfigureAwait(false);

        LogDebugEx(Result?.ToString().BoxFixedWidth($"About server", GlobalSettings.DEBUG_BOX_WIDTH));
        return Result;

      }
    } catch (Exception ex) {
      LogError($"Unable to get about of server services : {ex.Message}");
      if (ex.InnerException is not null) {
        LogError($"  Inner exception : {ex.InnerException.Message}");
        LogError($"  Inner call stack : {ex.InnerException.StackTrace}");
      }
      return null;
    }
  }

  
}
