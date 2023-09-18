namespace MediaSearch.Models.Support;
public interface IPropertiesFinder {
  Task Init();
  Task<string> GetVersion();
  IEnumerable<KeyValuePair<string, string>> GetAudioStreams();
  IEnumerable<KeyValuePair<string, string>> GetVideoStreams();
  IEnumerable<KeyValuePair<string, string>> GetSubTitleStreams();

}
