namespace MediaSearch.Models;

public interface IUserAccount : IUserAccountInfo {

  IUserAccountSecret Secret { get; }
}
