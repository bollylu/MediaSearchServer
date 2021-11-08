using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchModels {
  public class XFileInfo : IFileInfo {
    private FileInfo _Data;

    public string Name => _Data.Name;
    public string FullName => _Data.FullName;
    public DirectoryInfo Directory => _Data.Directory;
    public string DirectoryName => _Data.DirectoryName;
    public bool Exists => _Data.Exists;
    public bool IsReadOnly => _Data.IsReadOnly;
    public long Length { get; set; }

    public XFileInfo(string filename) {
      _Data = new FileInfo(filename);
    }
  }
}
