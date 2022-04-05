namespace MediaSearch.Models;

public interface IMediaInfoFile {
  string StoragePath { get; }
  string StorageName { get; }

  bool Exists();
  bool Remove();

  bool Read();
  bool Write();
  bool Export(string newFilename);

  Task<bool> ReadAsync(CancellationToken token);
  Task<bool> WriteAsync(CancellationToken token);
  Task<bool> ExportAsync(string newFilename, CancellationToken token);

  IMediaInfoHeader Header { get; }
  IMediaInfoContent Content { get; }

}
