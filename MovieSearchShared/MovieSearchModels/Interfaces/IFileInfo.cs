using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchModels {
  public interface IFileInfo {

    string Name { get; }
    string FullName { get; }
    DirectoryInfo Directory { get; }
    string DirectoryName { get; }
    bool Exists { get; }
    bool IsReadOnly { get; }
    long Length { get; }

  }
}
