namespace MediaSearch.Models;

public interface IAbout : IName {
  Version CurrentVersion { get; }
  string ChangeLog { get; }

  Task Initialize();
}
