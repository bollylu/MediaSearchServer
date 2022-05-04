using BLTools;

namespace MediaSearch.Database;
public class TMSTableJson<T, R> : IMSTableJson<T, R>, IMediaSearchLoggable<TMSTableJson<T, R>> 
  where T : IMSTable<R> 
  where R : IMedia {

  public const int TIMEOUT_IN_MS = 5000;
  public const string JSON_HEADER = "header";
  public const string JSON_CONTENT = "medias";

  public const string HEADER_FILE_EXTENSION = ".header.json";
  public const string RECORD_FILE_EXTENSION = ".record.json";

  public IMediaSearchLogger<TMSTableJson<T, R>> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMSTableJson<T, R>>();

  public string TablePath { get; init; } = "";
  public string TableName { get; init; } = "";
  public string TableFullName => Path.Join(TablePath, TableName);

  public string HeaderFilename => $"{TableName}{HEADER_FILE_EXTENSION}";
  public string HeaderFullFilename => Path.Join(TablePath, TableName, $"{TableName}{HEADER_FILE_EXTENSION}");

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMSTableJson() { }
  public TMSTableJson(string tablePath, string tableName) {
    TablePath = tablePath;
    TableName = tableName;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public bool Save(T table) {
    using (TChrono Chrono = new()) {
      Chrono.Start();

      try {
        Logger.IfDebugMessageEx($"Saving header to {HeaderFullFilename.WithQuotes()}");

        try {
          using (FileStream OutputStream = new FileStream(HeaderFullFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)) {
            using (Utf8JsonWriter Writer = new Utf8JsonWriter(OutputStream, new JsonWriterOptions() { Indented = true, Encoder = IJson.DefaultJsonSerializerOptions.Encoder })) {
              JsonSerializer.Serialize(Writer, table.Header, IJson.DefaultJsonSerializerOptions);
              Writer.Flush();
            }
          }
        } catch (Exception ex) {
          Logger.LogErrorBox($"Unable to save header to {HeaderFullFilename.WithQuotes()}", ex);
          return false;
        }

      } finally {
        Chrono.Stop();
        Logger.IfDebugMessageExBox($"Header saved to {HeaderFullFilename.WithQuotes()}", $"{Chrono.ElapsedTime.DisplayTime()}");
      }

      return true;
    }
  }
  public bool Save(R record) {
    using (TChrono Chrono = new()) {
      Chrono.Start();
      string RecordName = $"{Path.Combine(TableFullName, record.Id)}.record.json";

      try {
        Logger.IfDebugMessageEx($"Saving movie to {record.Id}");

        try {
          using (FileStream OutputStream = new FileStream(RecordName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)) {
            using (Utf8JsonWriter Writer = new Utf8JsonWriter(OutputStream, new JsonWriterOptions() { Indented = true, Encoder = IJson.DefaultJsonSerializerOptions.Encoder })) {
              JsonSerializer.Serialize(Writer, record, IJson.DefaultJsonSerializerOptions);
              Writer.Flush();
            }
          }
        } catch (Exception ex) {
          Logger.LogErrorBox($"Unable to save data to {RecordName.WithQuotes()}", ex);
          return false;
        }

      } finally {
        Chrono.Stop();
        Logger.IfDebugMessageExBox($"Record saved to {RecordName}", $"{Chrono.ElapsedTime.DisplayTime()}");
      }

      return true;
    }

  }

}
