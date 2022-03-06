using Microsoft.AspNetCore.Components;
using BLTools.Encryption;

namespace MediaSearch.Client.Components;

public partial class About_Card : ComponentBase {

  [Parameter]
  public TAbout About {
    get {
      return _About ??= new TAbout();
    }
    set {
      _About = value;
    }
  }
  private TAbout? _About;
    
  private bool IsCollapsed = false;
  private string Id => About.ChangeLog.HashToBase64().Left(10);
  [Parameter] public string Parent {get;set;} = string.Empty;
  private string CollapsableChild => $"#{Id}";

  private string AboutHeader => $"{About.Name} v{About.CurrentVersion}";


  private void ToggleBody() {
    IsCollapsed = !IsCollapsed;
  }
}
