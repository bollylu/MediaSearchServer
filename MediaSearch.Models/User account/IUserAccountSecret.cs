namespace MediaSearch.Models;

public interface IUserAccountSecret : IJson<IUserAccountSecret> {

  string Name { get; set; }

  string Password { get; set; }
  
  bool MustChangePassword { get; set; }

  IUserToken Token { get; set; }

}
