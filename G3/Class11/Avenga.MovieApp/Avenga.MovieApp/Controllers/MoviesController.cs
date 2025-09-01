using Avenga.MovieApp.DataAccess;
using Avenga.MovieApp.Domain.Enums;
using Avenga.MovieApp.Dtos;
using Avenga.MovieApp.Services.Interfaces;
using Avenga.MovieApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Avenga.MovieApp.Controllers
{
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
                return Ok(_movieService.GetAllMovies(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<MovieDto> GetMovieById(int id)
        {
            try
            {
                return Ok(_movieService.GetMovieById(id));
            }
            catch(MovieNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch(MovieException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
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
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (MovieException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateMovie([FromBody] UpdateMovieDto updateMovieDto) 
        {
            try
            {
                _movieService.UpdateMovie(updateMovieDto);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (MovieNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (MovieException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            try
            {
                _movieService.DeleteMovie(id);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (MovieNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (MovieException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("filterMovie")]
        public IActionResult FilterMovie(int? year, GenreEnum? genre) 
        {
            try
            {
                return Ok(_movieService.FilterMovies(year, genre));
            }
            catch (MovieException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        private int GetAuthorizedUserId()
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                throw new UserException(userId, User.FindFirst(ClaimTypes.Name)?.Value, "Name indentifire claims not exist!");
            }
            return userId;
        }
    }
}
