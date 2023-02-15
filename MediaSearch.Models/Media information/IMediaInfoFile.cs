namespace MediaSearch.Models;

public interface IMediaInfoFile {
  string StoragePath { get; }
  string StorageName { get; }

  string Name { get; }

  Task<bool> Exists();
  Task<bool> Read();
  Task<bool> Write();
  Task<bool> Export(string newFilename);

  IMediaInfoContent? Content { get; set; }
  ILanguageDictionary<string> GetTitles();
  ILanguageDictionary<string> GetDescription();
}
