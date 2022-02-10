using Microsoft.AspNetCore.Components;

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
    
}
