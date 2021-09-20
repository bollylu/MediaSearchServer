using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MovieSearchClient.Services;

using BLTools.Diagnostic.Logging;

using Microsoft.AspNetCore.Components;

namespace MovieSearchClient.Pages {

  public partial class Index : ComponentBase, ILoggable {

    [Inject]
    public IBusService<string> BusService { get; set; }

    private string CurrentFilter;

    #region --- ILoggable --------------------------------------------
    public ILogger Logger { get; set; }
    public void SetLogger(ILogger logger) {
      Logger = ALogger.Create(logger);
    }
    #endregion --- ILoggable --------------------------------------------

    protected override void OnInitialized() {
      // subscribe to OnMessage event
      SetLogger(ALogger.SYSTEM_LOGGER);
      BusService.OnMessage += _MessageHandler;
    }

    public void Dispose() {
      // unsubscribe from OnMessage event
      BusService.OnMessage -= _MessageHandler;
    }

    private void _MessageHandler(string source, string data) {
      switch (source) {

        case EditFilter.SVC_NAME: {
            Logger.Log($"Filter is now [{data}]");
            CurrentFilter = data;
            StateHasChanged();
          }
          break;

        default:
          Logger.Log($"Unknown source : {source}");
          StateHasChanged();
          break;
      }
    }
  }
}
