namespace MediaSearch.Models.Support;
public interface IPropertiesFinder {

  string Filename { get; }

  Task Init();
  Task<string> GetVersion();

  TMediaSourceStreamProperties MediaProperties { get; }

}
