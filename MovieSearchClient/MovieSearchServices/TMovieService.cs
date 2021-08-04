using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using MovieSearch.Models;

using BLTools.Diagnostic.Logging;

namespace MovieSearch.Services {

  /// <summary>
  /// Client Movie service. Provides access to groups, movies and pictures from a REST server
  /// </summary>

  public class TMovieService : ALoggable, IMovieService {

    #region --- Constants --------------------------------------------
    private int TIMEOUT = int.MaxValue;
    #endregion --- Constants --------------------------------------------

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    private readonly HttpClient Client;
    private readonly TImageCache ImageCache;

    public TMovieService(HttpClient client, TImageCache cache) {
      Client = client;
      ImageCache = cache;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public string RootPath { get; }

    public List<string> ExcludedExtensions { get; }

    #region --- Group actions --------------------------------------------
    public string CurrentGroup { get; }

    public async Task<IMovieGroups> GetGroups(string group = "/", string filter = "") {
      try {

        string RequestUrl = $"group?name={group.ToUrl()}&filter={filter.ToUrl()}";
        Log($"Requesting groups : {RequestUrl}");
        using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT)) {
          JsonSerializerOptions Options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
          IMovieGroups Result = await Client.GetFromJsonAsync<TMovieGroups>(RequestUrl, Options, Timeout.Token);
          Log($"Result : {Result.Name} - {Result.Groups.Count}");
          return Result;
        }
      } catch (Exception ex) {
        LogError($"Unable to get groups data : {ex.Message}");
        return null;
      }

    }
    #endregion --- Group actions --------------------------------------------

    #region --- Movie actions --------------------------------------------
    public async Task<IMovies> GetMovies(string group, string filter, int startPage = 0, int pageSize = 20) {
      try {
        string RequestUrl = $"movie?group={group.ToUrl()}&filter={filter.ToUrl()}&page={startPage}&size={pageSize}";
        Log($"Requesting movies : {RequestUrl}");
        using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT)) {
          JsonSerializerOptions Options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
          IMovies Result = await Client.GetFromJsonAsync<TMovies>(RequestUrl, Options, Timeout.Token);

          Console.WriteLine(Result.ToString());
          Log($"Result : {Result.Source} - {Result.Movies.Count}");
          return Result;
        }
      } catch (Exception ex) {
        LogError($"Unable to get movies data : {ex.Message}");
        return null;
      }
    }
    #endregion --- Movie actions --------------------------------------------

    #region --- Picture actions --------------------------------------------
    public async Task<byte[]> GetPicture(string pathname) {
      try {
        string RequestUrl = $"movie/getPicture?pathname={WebUtility.UrlEncode(pathname)}";
        Log($"Requesting picture : {RequestUrl}");
        using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT)) {
          JsonSerializerOptions Options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
          byte[] Result = await Client.GetByteArrayAsync(RequestUrl, Timeout.Token);
          return Result;
        }
      } catch (Exception ex) {
        LogError($"Unable to get movies data : {ex.Message}");
        return null;
      }
    }

    public async Task<string> GetPicture64(string pathname) {
      Console.WriteLine($"Loading picture in B64 : {pathname}");
      byte[] CachedPicture = ImageCache.GetImage(pathname);
      byte[] PictureBytes;
      if (CachedPicture==null) {
        PictureBytes = await GetPicture(pathname);
        ImageCache.AddImage(pathname, PictureBytes);
      } else {
        
        PictureBytes = new byte[CachedPicture.Length];
        CachedPicture.CopyTo(PictureBytes, 0);
      }
      
      if (PictureBytes != null) {
        return $"data:image/jpg;base64, {Convert.ToBase64String(PictureBytes)}";
      } else {
        return "";
      }
    }

    public string GetPictureLocation(string pathname) {
      string CompleteName = $"https://10.100.200.140:4567/api/movie/getPicture?pathname={WebUtility.UrlEncode(pathname)}";
      return CompleteName;
    }
    #endregion --- Picture actions --------------------------------------------
  }


}
