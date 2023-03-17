using BLTools.Text;

namespace MediaSearch.Models;
public class TActor : ARecord, IArtist {

  public string Name { get; set; } = "";
  public string FirstName { get; set; } = "";
  public string LastName { get; set; } = "";
  public string Alias { get; set; } = "";
  public string Alias2 { get; set; } = "";
  public string Description { get; set; } = "";

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TActor() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TActor>();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder(base.ToString(indent));
    RetVal.AppendLine();
    RetVal.AppendLine($"{TextBox.Spaces(indent)}{nameof(Name)}={Name.WithQuotes()}");
    RetVal.AppendLine($"{TextBox.Spaces(indent)}{nameof(FirstName)}={FirstName.WithQuotes()}");
    RetVal.AppendLine($"{TextBox.Spaces(indent)}{nameof(LastName)}={LastName.WithQuotes()}");
    RetVal.AppendLine($"{TextBox.Spaces(indent)}{nameof(Alias)}={Alias.WithQuotes()}");
    RetVal.AppendLine($"{TextBox.Spaces(indent)}{nameof(Alias2)}={Alias2.WithQuotes()}");
    RetVal.AppendLine($"{TextBox.Spaces(indent)}{nameof(Description)}={Description.WithQuotes()}");
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}
