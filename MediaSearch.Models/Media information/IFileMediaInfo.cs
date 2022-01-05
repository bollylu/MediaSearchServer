using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaSearch.Models;

public interface IFileMediaInfo : IName {
  Dictionary<ELanguage, string> Titles { get; }
  Dictionary<ELanguage, string> Descriptions { get; }
  int Size { get; }

  bool Read();
  bool Write();
  bool Export(string newFilename);
}
