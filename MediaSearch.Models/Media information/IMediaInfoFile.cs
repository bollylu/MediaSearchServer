namespace MediaSearch.Models;

public interface IMediaInfoFile {
  string StoragePath { get; }
  string StorageName { get; }

  IMediaInfoContent Content { get; }

  bool Exists();
  bool Read();
  bool Write();
  bool Export(string newFilename);
}
