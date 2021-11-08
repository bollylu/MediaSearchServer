using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

using MovieSearchModels.Interfaces;

namespace MovieSearchModels {
  public abstract class AMedia : ALoggable, IMedia {

    #region --- IName --------------------------------------------
    public string Name { get; set; }
    public string Description { get; set; }
    #endregion --- IName --------------------------------------------

    #region --- IMultiNames --------------------------------------------
    public Dictionary<string, string> AltNames { get; } = new();
    #endregion --- IMultiNames --------------------------------------------

    #region --- ITags --------------------------------------------
    public List<string> Tags { get; } = new();
    #endregion --- ITags --------------------------------------------

    public string Filename { get; set; }
    public string FileExtension { get; set; }


    
    public string StorageRoot { get; set; }
    public string StoragePath { get; set; }
    
    #region --- Constructor(s) ---------------------------------------------------------------------------------
    protected AMedia() { }
    protected AMedia(IMedia media) {
      Name = media.Name;
      Description = media.Description;

      StorageRoot = media.StorageRoot;
      StoragePath = media.StoragePath;
      Filename = media.Filename;
      FileExtension = media.FileExtension;

      foreach(KeyValuePair<string, string> AltNameItem in media.AltNames) {
        AltNames.Add(AltNameItem.Key, AltNameItem.Value);
      }

      foreach (string TagItem in media.Tags) {
        Tags.Add(TagItem);
      }
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public override string ToString() {
      StringBuilder RetVal = new();
      RetVal.AppendLine($"{nameof(Name)} = {Name}");
      RetVal.AppendLine($"{nameof(Description)} = {Description}");
      RetVal.AppendLine($"{nameof(StorageRoot)} = {StorageRoot}");
      RetVal.AppendLine($"{nameof(StoragePath)} = {StoragePath}");
      RetVal.AppendLine($"{nameof(Filename)} = {Filename}");
      RetVal.AppendLine($"{nameof(FileExtension)} = {FileExtension}");
      if (AltNames.Any()) {
        RetVal.AppendLine("Alt. names");
        foreach (KeyValuePair<string, string> AltNameItem in AltNames) {
          RetVal.AppendLine($"|- {AltNameItem.Key}:{AltNameItem.Value}");
        }
      } else {
        RetVal.AppendLine($"{nameof(AltNames)} is empty");
      }
      if (Tags.Any()) {
        RetVal.AppendLine("Tags");
        foreach (string TagItem in Tags) {
          RetVal.AppendLine($"|- {TagItem}");
        }
      } else {
        RetVal.AppendLine($"{nameof(Tags)} is empty");
      }
      return RetVal.ToString();
    }

    //#region --- IJson --------------------------------------------
    //public virtual IJson ParseJson(string source) {
    //  throw new NotImplementedException();
    //}

    //public virtual IJson ParseJson(JsonElement source) {
    //  throw new NotImplementedException();
    //}

    //public virtual string ToJson() {
    //  throw new NotImplementedException();
    //}

    //public virtual string ToJson(JsonWriterOptions options) {
    //  throw new NotImplementedException();
    //} 
    //#endregion --- IJson --------------------------------------------
  }
}
