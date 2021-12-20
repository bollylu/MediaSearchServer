namespace MovieSearchModels;

public class TFileInfo : IFileInfo {

  private FileInfo _Data;

  public string Name => _Data.Name;
  public string FullName => _Data.FullName;
  public DirectoryInfo Directory => _Data.Directory;
  public string DirectoryName => _Data.DirectoryName;
  public bool Exists => _Data.Exists;
  public bool IsReadOnly => _Data.IsReadOnly;
  public long Length => _Data.Length;

  public DateTime CreationDate => _Data.CreationTime;
  public DateTime ModificationDate => _Data.LastWriteTime;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TFileInfo(string filename) {
    _Data = new FileInfo(filename);
  }

  public TFileInfo(FileInfo fileInfo) {
    _Data = fileInfo;
  } 
  #endregion --- Constructor(s) ------------------------------------------------------------------------------
}
