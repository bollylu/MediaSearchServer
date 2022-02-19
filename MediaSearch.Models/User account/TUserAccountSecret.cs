namespace MediaSearch.Models;
public class TUserAccountSecret : ADataModel, IUserAccountSecret, IJson<TUserAccountSecret> {

  //[JsonPropertyName(nameof(Name))]
  public string Name { get; set; } = "";

  //[JsonPropertyName(nameof(Password))]
  public string Password { get; set; } = "";

  //[JsonPropertyName(nameof(MustChangePassword))]
  public bool MustChangePassword { get; set; } = false;

  //[JsonPropertyName(nameof(Token))]
  public IUserToken Token { get; set; } = TUserToken.ExpiredUserToken;

}
