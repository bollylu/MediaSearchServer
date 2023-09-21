using MediaSearch.Models.Support.ffprobe;

namespace MediaSearch.Models.Support;
public interface IPropertiesFinder {

  string Filename { get; }

  Task Init();
  Task<string> GetVersion();

  TMediaProperties MediaProperties { get; }

}
