using BLTools.Text;

namespace MediaSearch.Models;

public abstract class ARecord : ALoggable, IRecord {

  public virtual string Id { get; protected set; } = string.Empty;

  #region --- IDirty --------------------------------------------
  public bool IsDirty { get; protected set; } = false;

  public virtual void SetDirty() {
    IsDirty = true;
  }

  public virtual void ClearDirty() {
    IsDirty = false;
  }
  #endregion --- IDirty --------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  /// <summary>
  /// Extended version of ToString with an optional indent size
  /// </summary>
  /// <param name="indent">The number of spaces to indent content</param>
  /// <returns>A string representation of the object</returns>
  public virtual string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.Append($"{TextBox.Spaces(indent)}{Id}");
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }

  #endregion --- Converters -------------------------------------------------------------------------------------
}
