using Avenga.MovieApp.DataAccess;
using Avenga.MovieApp.Domain.Enums;
using Avenga.MovieApp.Dtos;
using Avenga.MovieApp.Services.Interfaces;
using Avenga.MovieApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace Avenga.MovieApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public ActionResult<List<MovieDto>> GetAllMovie()
        {
            try
            {
                int userId = GetAuthorizedUserId();
                //Log.Information("OK: Succ get all movies");
                return Ok(_movieService.GetAllMovies(userId));
            }
            catch (Exception ex)
            {
                Log.Error("Internal server: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<MovieDto> GetMovieById(int id)
        {
            try
            {
                var movie = _movieService.GetMovieById(id);
                Log.Information($"OK: {movie.Title}");
                return Ok(movie);
            }
            catch(MovieNotFoundException e)
            {
                Log.Error($"Not Found: Movie with id {id} was not found!");
                return NotFound(e.Message);
            }
            catch(MovieException e)
            {
                Log.Error($"BadRequest: " + e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Internal server: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddMovie([FromBody] AddMovieDto addMovieDto)
        {
            try
            {
                int userId = GetAuthorizedUserId();
                _movieService.AddMovie(addMovieDto, userId);
                Log.Information($"User with id {userId} succ added new movie!");
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (MovieException e)
            {
                Log.Error("MovieException: " + e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Internal server: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateMovie([FromBody] UpdateMovieDto updateMovieDto) 
        {
            try
            {
                int userId = GetAuthorizedUserId();
                _movieService.UpdateMovie(updateMovieDto);
                Log.Information($"User with id {userId} succ updated movie!");
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (MovieNotFoundException e)
            {
                Log.Error("MovieNotFoundException: " + e.Message);
                return NotFound(e.Message);
            }
            catch (MovieException e)
            {
                Log.Error("MovieException: " + e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Internal server: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            try
            {
                int userId = GetAuthorizedUserId();
                _movieService.DeleteMovie(id);
                Log.Information($"User with id:{userId} succ deleted movie with id: {id} ");
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (MovieNotFoundException e)
            {
                Log.Error("MovieNotFoundException: " + e.Message);
                return NotFound(e.Message);
            }
            catch (MovieException e)
            {
                Log.Error("MovieException: " + e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Internal server: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("filterMovie")]
        public IActionResult FilterMovie(int? year, GenreEnum? genre) 
        {
            try
            {
                var filterMovie = _movieService.FilterMovies(year, genre);
                return Ok(filterMovie);
            }
            catch (MovieException e)
            {
                Log.Error("MovieException: " + e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Internal server: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        private int GetAuthorizedUserId()
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                Log.Error($"User Exception: Name indentifire claims not exist! With name: {User.FindFirst(ClaimTypes.Name)?.Value}");
                throw new UserException(userId, User.FindFirst(ClaimTypes.Name)?.Value, "Name indentifire claims not exist!");
            }
            return userId;
        }
    }
}
