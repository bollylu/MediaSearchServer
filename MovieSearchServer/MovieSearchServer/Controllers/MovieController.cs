using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using MovieSearch.Models;

using MovieSearchServerServices.MovieService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using BLTools.Diagnostic.Logging;
using System.Text;
using System.Text.Json;

namespace MovieSearchServer.Controllers {
  [ApiController]
  [Route("api/movie")]
  public class TMovieController : ControllerBase, ILoggable {

    private readonly IMovieService _MovieService;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TMovieController(IMovieService movieService, ILogger logger) {
      _MovieService = movieService;
      SetLogger(logger);
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    #region --- ILoggable --------------------------------------------
    public ILogger Logger { get; set; }
    [NonAction]
    public void SetLogger(ILogger logger) {
      if (logger is null) {
        Logger = ALogger.DEFAULT_LOGGER;
      } else {
        Logger = ALogger.Create(logger);
      }
    }
    #endregion --- ILoggable --------------------------------------------

    ///// <summary>
    ///// Obtain the list of movies for a given group, with the possibility of filtering and pagination
    ///// </summary>
    ///// <param name="group">The group name</param>
    ///// <param name="filter">A possible filter for the movie names</param>
    ///// <param name="page">The first page (x items count)</param>
    ///// <param name="items">The items count for the request</param>
    ///// <returns>A IMovies object containing the data</returns>
    //[HttpGet()]
    //public async Task<ActionResult<IMovies>> Get(string group = "", string filter = "", int page = 1, int items = 20) {
    //  Console.WriteLine($"GetMovies for group {WebUtility.UrlDecode(group)}, filter={WebUtility.UrlDecode(filter)}, page={page}, items={items}");
    //  return new JsonResult(await MovieService.GetMovies(WebUtility.UrlDecode(group), WebUtility.UrlDecode(filter), page, items).ToListAsync());
    //}

    /// <summary>
    /// Obtain a page of movies with the possibility of filtering
    /// </summary>
    /// <param name="filter">A possible filter for the movie names</param>
    /// <param name="page">The first page (x items count)</param>
    /// <param name="items">The items count for the request</param>
    /// <returns>A IMovies object containing the data</returns>
    [HttpGet()]
    public async Task<ActionResult<IMovies>> Get(string filter = "", int page = 1, int items = 20) {
      Logger?.Log($"New request : {HttpContext.Request.QueryString}");
      Logger?.Log($"Origin : {HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.RemotePort}");
      if (string.IsNullOrWhiteSpace(filter)) {
        Logger?.Log($"GetMovies : page={page}, items={items}");

      } else {
        Logger?.Log($"GetMovies : filter={WebUtility.UrlDecode(filter)}, page={page}, items={items}");
      }

      IMovies RetVal = new TMovies() {
        Source = _MovieService.Storage,
        Page = page,
        AvailablePages = await _MovieService.PagesCount(items).ConfigureAwait(false)
      };

      await foreach(TMovie MovieItem in _MovieService.GetMovies(WebUtility.UrlDecode(filter), page, items).ConfigureAwait(false)) {
        RetVal.Movies.Add(MovieItem);
      }

      Logger?.Log($"Returning {RetVal.Movies.Count} movies");
      Logger?.Log(_PrintMovies(RetVal.Movies));

      return new ActionResult<IMovies>(RetVal);
    }



    //[HttpGet("getPicture")]
    //public async Task<FileContentResult> GetPicture(string pathname) {
    //  byte[] Result = await MovieService.GetPicture(WebUtility.UrlDecode(pathname));
    //  return File(Result, "image/jpeg");
    //}

    #region --- Support --------------------------------------------
    private string _PrintMovies(IEnumerable<IMovie> movies) {
      StringBuilder RetVal = new();

      foreach (IMovie MovieItem in movies) {
        RetVal.AppendLine($"  {MovieItem.LocalName}");
      }
      return RetVal.ToString();
    }
    #endregion --- Support --------------------------------------------
  }
}
