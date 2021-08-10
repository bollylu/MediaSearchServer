using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using MovieSearch.Models;

using MovieSearchServerServices.MovieService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace MovieSearchServer.Controllers {
  [ApiController]
  [Route("api/[controller]")]
  public class MovieController : ControllerBase {

    private readonly IMovieService MovieService;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public MovieController(IMovieService movieService) {
      MovieService = movieService;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    /// <summary>
    /// Obtain the list of movies for a given group, with the possibility of filtering and pagination
    /// </summary>
    /// <param name="group">The group name</param>
    /// <param name="filter">A possible filter for the movie names</param>
    /// <param name="page">The first page (x items count)</param>
    /// <param name="items">The items count for the request</param>
    /// <returns>A IMovies object containing the data</returns>
    [HttpGet()]
    public async Task<ActionResult<IMovies>> Get(string group = "", string filter = "", int page = 1, int items = 20) {
      Console.WriteLine($"GetMovies for group {WebUtility.UrlDecode(group)}, filter={WebUtility.UrlDecode(filter)}, page={page}, items={items}");
      return new JsonResult(await MovieService.GetMovies(WebUtility.UrlDecode(group), WebUtility.UrlDecode(filter), page, items).ToListAsync());
    }

    [HttpGet("getPicture")]
    public async Task<FileContentResult> GetPicture(string pathname) {
      byte[] Result = await MovieService.GetPicture(WebUtility.UrlDecode(pathname));
      return File(Result, "image/jpeg");
    }
    
  }
}
