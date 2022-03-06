namespace MediaSearch.Models;
public interface IUserToken : IJson<IUserToken>  {

  public string TokenId { get; set; }
  public DateTime Expiration { get; set; }

  bool IsExpired => Expiration < DateTime.UtcNow;

  void Duplicate(IUserToken userToken);

}
