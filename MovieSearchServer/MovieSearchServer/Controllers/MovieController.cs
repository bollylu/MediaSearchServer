using Microsoft.AspNetCore.Mvc;

using MovieSearchModels;

using System.Net;

namespace MovieSearchServer.Controllers;

[ApiController]
[Route("api/movie")]
public class TMovieController : AController
{

    private readonly IMovieService _MovieService;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TMovieController(IMovieService movieService, ILogger logger) : base(logger)
    {
        _MovieService = movieService;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    /// <summary>
    /// Obtain a page of movies with the possibility of filtering
    /// </summary>
    /// <param name="filter">A possible filter for the movie names</param>
    /// <param name="page">The first page (x items count)</param>
    /// <param name="items">The items count for the request</param>
    /// <returns>A IMovies object containing the data</returns>
    [HttpGet()]
    public async Task<ActionResult<IMoviesPage>> Get(string filter = "", int page = 1, int items = 20)
    {
        Logger?.Log($"New request : {HttpContext.Request.QueryString}");
        Logger?.Log($"Origin : {HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.RemotePort}");

        IMoviesPage RetVal;

        if (string.IsNullOrWhiteSpace(filter))
        {
            Logger?.Log($"GetMovies : page={page}, items={items}");
            RetVal = new TMoviesPage()
            {
                Source = _MovieService.RootStoragePath,
                Page = page,
                AvailablePages = await _MovieService.PagesCount(items).ConfigureAwait(false),
                AvailableMovies = await _MovieService.MoviesCount()
            };
        }
        else
        {
            Logger?.Log($"GetMovies : filter={WebUtility.UrlDecode(filter)}, page={page}, items={items}");
            RetVal = new TMoviesPage()
            {
                Source = _MovieService.RootStoragePath,
                Page = page,
                AvailablePages = await _MovieService.PagesCount(WebUtility.UrlDecode(filter), items).ConfigureAwait(false),
                AvailableMovies = await _MovieService.MoviesCount(WebUtility.UrlDecode(filter))
            };
        }

        await foreach (TMovie MovieItem in _MovieService.GetMovies(WebUtility.UrlDecode(filter), page, items).ConfigureAwait(false))
        {
            RetVal.Movies.Add(MovieItem);
        }

        Logger?.Log($"Returning {RetVal.Movies.Count} movies");
        Logger?.Log(_PrintMovies(RetVal.Movies));

        return new ActionResult<IMoviesPage>(RetVal);
    }



    [HttpGet("getPicture")]
    public async Task<ActionResult> GetPicture(string id, int width, int height)
    {
        byte[] Result = await _MovieService.GetPicture(id: WebUtility.UrlDecode(id),
                                                       width: width,
                                                       height: height);
        if (Result is null)
        {
            return new NotFoundResult();
        }
        else
        {
            return File(Result, "image/jpeg");
        }
    }

    #region --- Support --------------------------------------------
    private string _PrintMovies(IEnumerable<IMovie> movies)
    {
        StringBuilder RetVal = new();

        foreach (IMovie MovieItem in movies)
        {
            RetVal.AppendLine($"  {MovieItem.FileName}");
        }
        return RetVal.ToString();
    }
    #endregion --- Support --------------------------------------------
}

