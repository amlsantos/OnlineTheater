﻿using Logic.Movies;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Movies;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    public MoviesController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    [HttpGet("[action]")]
    public IActionResult Get(long id)
    {
        var movie = _unitOfWork.Movies.GetById(id);
        if (movie is null)
            return NotFound();

        var dto = new MovieDto
        {
            Id = movie.Id,
            Name = movie.Name
        };

        return Ok(dto);
    }

    [HttpGet("[action]")]
    public IActionResult GetList()
    {
        var movies = _unitOfWork.Movies.GetList();
        var dtos = movies.Select(m => new MovieInListDto
        {
            Id = m.Id,
            Name = m.Name,
            LicenseModel = m.LicensingModel.Name()
        }).ToList();

        return Ok(dtos);
    }
}