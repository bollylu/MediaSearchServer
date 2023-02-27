namespace MediaSearch.Models;
public interface IMediaSourceParser {

  IMedia? ParseRow(IFileInfo data, string rootStoragePath);
  Task<IMedia?> ParseRowAsync(IFileInfo data, string rootStoragePath, CancellationToken token);

}
