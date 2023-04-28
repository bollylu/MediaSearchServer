using MediaSearch.Models;

namespace MediaSearch.Models2;

public interface IAbout : IName {
  Version CurrentVersion { get; }
  string ChangeLog { get; }

  Task Initialize();
}
