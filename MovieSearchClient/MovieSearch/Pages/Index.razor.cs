using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MovieSearch.Client.Services;

using BLTools.Diagnostic.Logging;

using Microsoft.AspNetCore.Components;

namespace MovieSearch.Pages {



  public partial class Index : ComponentBase, ILoggable {

    [Inject]
    public IBusService<string> BusService { get; set; }

    private string CurrentGroup;
    private string CurrentFilter;

    public ILogger Logger { get; set; }
    public void SetLogger(ILogger logger) {
      Logger = ALogger.Create(logger);
    }

    private void Refresh(string group) {
      CurrentGroup = group;
      Logger.Log($"Refresh group.Name = {group}");
    }

    protected override void OnInitialized() {
      // subscribe to OnMessage event
      SetLogger(ALogger.SYSTEM_LOGGER);
      BusService.OnMessage += MessageHandler;
    }

    public void Dispose() {
      // unsubscribe from OnMessage event
      BusService.OnMessage -= MessageHandler;
    }

    private void MessageHandler(string source, string data) {
      switch (source) {
        case Groups.SVC_NAME: {
            if (data != null) {
              CurrentGroup = data;
              Logger.Log(data);
              StateHasChanged();
            }
            break;
          }
        case EditFilter.SVC_NAME: {
            Console.WriteLine($"Filter is now [{data}]");
            CurrentFilter = data;
            StateHasChanged();
          }
          break;

      }
    }
  }
}
