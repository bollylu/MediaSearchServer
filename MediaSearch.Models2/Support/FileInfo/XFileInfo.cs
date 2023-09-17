using System.Reflection;
using System.Runtime.InteropServices;

namespace MediaSearch.Models;

/// <summary>
/// Mockup class for FileInfo, without real disk access
/// </summary>
public class XFileInfo : IFileInfo {
  private readonly string _RawData;
  private readonly bool _IsWindows;
  private readonly char _Separator;

  public string Root { get; init; } = string.Empty;
  public string Name => _RawData.AfterLast(_Separator);
  public string FullName {
    get {
      return Path.Join(Root, DirectoryName, Name);
    }
  }

  public DirectoryInfo? Directory { get; init; }
  public string DirectoryName => _RawData.After(_Separator).Before(_Separator);
  public bool Exists { get; init; } = true;
  public bool IsReadOnly { get; init; } = true;
  public long Length { get; init; }
  public DateTime CreationDate { get; init; } = DateTime.MinValue;
  public DateTime ModificationDate { get; init; } = DateTime.MinValue;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public XFileInfo(string filename, string root = "") : this(OSPlatform.Windows, filename, root) {
  }

  public XFileInfo(OSPlatform osPlatform, string filename, string root = "") {
    Root = root;
    _RawData = filename;
    _IsWindows = osPlatform == OSPlatform.Windows ? true : false;
    _Separator = _IsWindows ? '\\' : '/';
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    foreach (var FieldInfoItem in typeof(XFileInfo).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(x => !x.Name.StartsWith('<'))) {
      if (FieldInfoItem.FieldType == typeof(string)) {
        RetVal.AppendLine($"{FieldInfoItem.Name} = {FieldInfoItem?.GetValue(this)?.ToString()?.WithQuotes()}");
      } else if (FieldInfoItem.FieldType == typeof(char)) {
        RetVal.AppendLine($"{FieldInfoItem.Name} = '{FieldInfoItem.GetValue(this)}'");
      } else {
        RetVal.AppendLine($"{FieldInfoItem.Name} = {FieldInfoItem.GetValue(this)}");
      }
    }
    foreach (var PropertyInfoItem in typeof(XFileInfo).GetProperties()) {
      if (PropertyInfoItem.PropertyType == typeof(string)) {
        RetVal.AppendLine($"{PropertyInfoItem.Name} = {PropertyInfoItem?.GetValue(this)?.ToString()?.WithQuotes()}");
      } else {
        RetVal.AppendLine($"{PropertyInfoItem.Name} = {PropertyInfoItem.GetValue(this)}");
      }
    }
    return RetVal.ToString();
  }
}
