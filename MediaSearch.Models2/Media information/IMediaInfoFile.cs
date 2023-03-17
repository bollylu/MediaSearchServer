namespace MediaSearch.Models;

public interface IMediaInfoFile {
  string StoragePath { get; }
  string StorageName { get; }

  string Name { get; }

  bool Exists();
  bool Read();
  bool Write();


  ValueTask<bool> ExistsAsync(CancellationToken token);
  ValueTask<bool> ReadAsync(CancellationToken token);
  ValueTask<bool> WriteAsync(CancellationToken token);
  ValueTask<bool> ExportAsync(string newFilename, CancellationToken token);

  IMediaInfoContent? Content { get; set; }
  ILanguageDictionary<string> GetTitles();
  ILanguageDictionary<string> GetDescription();
}
