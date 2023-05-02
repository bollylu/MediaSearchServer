namespace MediaSearch.Models;

public interface IFileInfo {

  string Name { get; }
  string FullName { get; }
  DirectoryInfo? Directory { get; }
  string? DirectoryName { get; }
  bool Exists { get; }
  bool IsReadOnly { get; }
  long Length { get; }

  DateTime CreationDate { get; }
  DateTime ModificationDate { get; }

}
