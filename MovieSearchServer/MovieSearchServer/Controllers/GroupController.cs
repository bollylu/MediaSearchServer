﻿using Microsoft.AspNetCore.Mvc;

namespace MovieSearchServer.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public class GroupController : ControllerBase
{

    private readonly IMovieService MovieService;

    public GroupController(IMovieService movieService)
    {
        MovieService = movieService;
    }

    //[HttpGet]
    //public async Task<ActionResult<IMovieGroups>> Get(string name, string filter) {
    //  Console.WriteLine($"Get groups {name.FromUrl()} - {filter.FromUrl()}");

    //  return new JsonResult(await MovieService.GetGroups(name.FromUrl(), filter.FromUrl()));
    //}

}

