namespace MediaSearch.Models;

public interface IUserAccount : IUserAccountInfo {

  TUserAccountSecret Secret { get; set; }
}
