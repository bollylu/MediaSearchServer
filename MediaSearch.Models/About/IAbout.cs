namespace MediaSearch.Models;

public interface IAbout : IName, IJson<IAbout> {
  Version CurrentVersion { get; }
  string ChangeLog { get; }

  Task Initialize();
}
