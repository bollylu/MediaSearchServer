namespace MediaSearch.Models;
public interface IUserToken  {

  public string Token { get; set; }
  public DateTime Expiration { get; set; }

  bool IsExpired => Expiration < DateTime.UtcNow;

}
