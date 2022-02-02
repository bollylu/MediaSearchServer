namespace MediaSearch.Server.Services;

/// <summary>
/// Mockup class for FileInfo, without real disk access
/// </summary>
public class XFileInfo : IFileInfo {
  private FileInfo _Data;

  public string Name => _Data.Name;
  public string FullName => _Data.FullName;
  public DirectoryInfo? Directory => _Data.Directory;
  public string? DirectoryName => _Data.DirectoryName;
  public bool Exists { get; set; } = true;
  public bool IsReadOnly { get; set; } = true;
  public long Length { get; set; }
  public DateTime CreationDate => _Data.CreationTime;
  public DateTime ModificationDate => _Data.LastWriteTime;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public XFileInfo(string filename) {
    _Data = new FileInfo(filename);
  } 
  #endregion --- Constructor(s) ------------------------------------------------------------------------------
}
