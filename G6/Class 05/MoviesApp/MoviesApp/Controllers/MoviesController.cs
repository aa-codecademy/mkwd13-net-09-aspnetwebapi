
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Models;
using MoviesApp.Models.DTOs;
using MoviesApp.Models.Enums;

namespace MoviesApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MoviesController : ControllerBase //http://localhost:[port]/api/Movies
	{
		[HttpGet] //http://localhost:[port]/api/Movies
		public ActionResult<List<MovieDto>> GetAll()
		{
			try
			{
				var moviesDb = StaticDb.Movies;
				if(moviesDb == null)
				{
					return Ok(new List<MovieDto>()); //we haven't added any movies yet  - not an error, just an empty collection
				}

				var moviesDto = moviesDb.Select(x => new MovieDto //we need to map the data from the db (Movie domain class) into MovieDto
				{
					Title = x.Title,
					Description	= x.Description,
					Year = x.Year,
					Genre = x.Genre
				});

				return Ok(moviesDto);
			}catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		//we must send the path param because it is a part of the basic route - it differences this route from the other routes
		//if we don't send the path param - then this endpoint cannot be hit (it will go to the route //http://localhost:[port]/api/Movies (GetAll))
		[HttpGet("{id}")] //http://localhost:[port]/api/Movies/1
		public ActionResult<MovieDto> GetByPathId(int id)
		{
			try
			{
				//validations
				if(id <= 0)
				{
					return BadRequest("Bad request, the id cannot be a negative number");
				}

				var movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == id);
				if(movieDb == null)
				{
					return NotFound($"Movie with id {id} was not found");
				}

				//map
				var movieDto = new MovieDto
				{
					Description = movieDb.Description,
					Year = movieDb.Year,
					Genre = movieDb.Genre,
					Title = movieDb.Title
				};

				return Ok(movieDto);

			}catch(Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		//it is totally valid to not send a query param - because a query param is not a part of the basic route, but it is added to the basic route - we can still hit this endpoint without the query param
		[HttpGet("queryString")] //http://localhost:[port]/api/Movies/queryString - this is the route without the query string part - this is the basic route/url for this method
								 //http://localhost:[port]/api/Movies/queryString?id=1 -we can add to the basic route for this method a query string where we can send the params
		public ActionResult<MovieDto> GetByQueryStringId(int? id)
		{
			try
			{
				//validations
				if(id == null)
				{
					return BadRequest("The id cannot be null");
				}
				if (id <= 0)
				{
					return BadRequest("Bad request, the id cannot be a negative number");
				}

				var movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == id);
				if (movieDb == null)
				{
					return NotFound($"Movie with id {id} was not found");
				}

				//map
				var movieDto = new MovieDto
				{
					Description = movieDb.Description,
					Year = movieDb.Year,
					Genre = movieDb.Genre,
					Title = movieDb.Title
				};

				return Ok(movieDto);

			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("filter")]  //http://localhost:[port]/api/movies/filter
							 //http://localhost:[port]/api/movies/filter?genre=1
							 //http://localhost:[port]/api/movies/filter?year=2022
							 //http://localhost:[port]/api/movies/filter?year=2022&genre=1
							 //http://localhost:[port]/api/movies/filter?genre=1&&year=2022
		public ActionResult<List<MovieDto>> FilterMovies(int? genre, int? year)
		{
			try
			{
				if(genre == null && year == null) //the user  is allowed to skip the query string params and the route would still be valid, however from business logic aspect we need at least one param so that we can filter
				{
					return BadRequest("You have to send at least one filter param");
				}

				//check if the genre is in the correct range for the genre enum - only when genre has value (a value for genre was sent)
				if (genre.HasValue) //HasValue checks if genre is not null (has value)
				{
					var enumValues = Enum.GetValues(typeof(GenreEnum)) //returns  array of the values as type Enum
									 .Cast<GenreEnum>()//we need our specific type of enum - GenreEnum, not the base Enum type, so we need to cast the Enum array into GenreEnum -> Comedy=1, Action=2....
									 .Select(genre => (int)genre) //1,2,3...
									 .ToList();

					//genre is nullable, we need .Value to access its value if it was sent
					if (!enumValues.Contains(genre.Value)) //ex.we sent 15 as genre
					{
						return NotFound($"The genre with id {genre.Value} was not found");
					}
									 } 

				if(year == null) //here we can be sure that the genre has value, because we already checked the scenario where both of them dont have values. So, if year does not have value - then genre must have a value
				{
					List<Movie> moviesDbByGenre = StaticDb.Movies.Where(x => (int)x.Genre == genre.Value).ToList();
					//List<Movie> moviesDbByGenre = StaticDb.Movies.Where(x => x.Genre == (GenreEnum)genre.Value).ToList();

					var moviesGenreDto = moviesDbByGenre.Select(x => new MovieDto
					{
						Description = x.Description,
						Genre = x.Genre,
						Year = x.Year,
						Title = x.Title
					});

					return Ok(moviesGenreDto);
				}

				if(genre == null)//here we can be sure that the year has value, because we already checked the scenario where both of them dont have values. So, if genre does not have value - then year must have a value
				{
					var moviesDbByYear = StaticDb.Movies.Where(x => x.Year == year).ToList();

					var moviesYearDto = moviesDbByYear.Select(x => new MovieDto
					{
						Description = x.Description,
						Genre = x.Genre,
						Year = x.Year,
						Title = x.Title
					});

					return Ok(moviesYearDto);
				}

				List<Movie> moviesDb = StaticDb.Movies.Where(x => (int)x.Genre == genre.Value && x.Year == year).ToList();
				var moviesDto = moviesDb.Select(x => new MovieDto
				{
					Description = x.Description,
					Genre = x.Genre,
					Year = x.Year,
					Title = x.Title
				});

				return Ok(moviesDto);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost("addMovie")] //here we don't have to add anything to the route, because we don't have another httppost method
		public IActionResult AddMovie([FromBody] AddMovieDto addMovieDto) //we use a specific dto, beacuse in the fiture some of the data might change for adding/showing the movies
		{
			try
			{
				//validations
				if (addMovieDto == null)
				{
					return BadRequest("Movie cannot be null");
				}

				//title is required
				if (string.IsNullOrEmpty(addMovieDto.Title))
				{
					return BadRequest("Title is required");
				}

				//If description is entered - maximum length is 250 chars
				//description was entered                             //and description had more than 250 chars
				if(!string.IsNullOrEmpty(addMovieDto.Description) && addMovieDto.Description.Length > 250)
				{
					return BadRequest("Description cannot be longer than 250 characters");
				}

				//there is no need to check if year has a null value, because a null value cannot be stored into a int (it can only be stored in int?)
				if(addMovieDto.Year <= 0 || addMovieDto.Year > DateTime.Now.Year)
				{
					return BadRequest("Invalid value for year");
				}

				var enumValues = Enum.GetValues(typeof(GenreEnum)) //returns  array of the values as type Enum
									 .Cast<GenreEnum>()//we need our specific type of enum - GenreEnum, not the base Enum type, so we need to cast the Enum array into GenreEnum -> Comedy=1, Action=2....
									 .Select(genre => (int)genre) //1,2,3...
									 .ToList();

				if (!enumValues.Contains((int)addMovieDto.Genre)) //ex.we sent 15 as genre
				{
					return NotFound($"The genre with id {(int)addMovieDto.Genre} was not found");
				}

				//mapping - we need an instance of Movie class
				Movie movie = new Movie
				{
					Id = StaticDb.Movies.LastOrDefault().Id + 1,
					Title = addMovieDto.Title,
					Description = addMovieDto.Description,
					Genre = addMovieDto.Genre,
					Year = addMovieDto.Year
				};

				//add the movie in db
				StaticDb.Movies.Add(movie);
				return StatusCode(StatusCodes.Status201Created);
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
				//validaions
				if(updateMovieDto == null)
				{
					return BadRequest("Movie cannot be null");
				}
				
				//if the updateMovieDto.Id is null or is a negative value or is a value that does not exist in StaticDb.Movies - the value of movieDb will be null, so we can only use this check
				Movie movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == updateMovieDto.Id);
				if(movieDb == null)
				{
					return NotFound($"Movie with id {updateMovieDto.Id} was not found");
				}

				//the same rules (requirements) that we had to check when creating a movie, we need to check when we update the movie as well
				//title is required
				if (string.IsNullOrEmpty(updateMovieDto.Title))
				{
					return BadRequest("Title is required");
				}

				//If description is entered - maximum length is 250 chars
				//description was entered                             //and description had more than 250 chars
				if (!string.IsNullOrEmpty(updateMovieDto.Description) && updateMovieDto.Description.Length > 250)
				{
					return BadRequest("Description cannot be longer than 250 characters");
				}

				//there is no need to check if year has a null value, because a null value cannot be stored into a int (it can only be stored in int?)
				if (updateMovieDto.Year <= 0 || updateMovieDto.Year > DateTime.Now.Year)
				{
					return BadRequest("Invalid value for year");
				}

				var enumValues = Enum.GetValues(typeof(GenreEnum)) //returns  array of the values as type Enum
									 .Cast<GenreEnum>()//we need our specific type of enum - GenreEnum, not the base Enum type, so we need to cast the Enum array into GenreEnum -> Comedy=1, Action=2....
									 .Select(genre => (int)genre) //1,2,3...
									 .ToList();

				if (!enumValues.Contains((int)updateMovieDto.Genre)) //ex.we sent 15 as genre
				{
					return NotFound($"The genre with id {(int)updateMovieDto.Genre} was not found");
				}

				//mapping - update
				movieDb.Title = updateMovieDto.Title;
				movieDb.Description = updateMovieDto.Description;
				movieDb.Genre = updateMovieDto.Genre;
				movieDb.Year = updateMovieDto.Year;

				return StatusCode(StatusCodes.Status204NoContent, "Note updated");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpDelete]
		public IActionResult DeleteMovie([FromBody] int id)
		{
			try
			{
				//validations
				if(id <= 0)
				{
					return BadRequest("Id cannot have a negative value");
				}

				var movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == id);
				if(movieDb == null)
				{
					return NotFound("Movie was not found");
				}

				//delete
				StaticDb.Movies.Remove(movieDb);
				return StatusCode(StatusCodes.Status204NoContent, "Movie was deleted");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteMovieByRouteId(int id)
		{
			try
			{
				//validations
				if (id <= 0)
				{
					return BadRequest("Id cannot have a negative value");
				}

				var movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == id);
				if (movieDb == null)
				{
					return NotFound("Movie was not found");
				}

				//delete
				StaticDb.Movies.Remove(movieDb);
				return StatusCode(StatusCodes.Status204NoContent, "Movie was deleted");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		//queryString
		[HttpDelete("query")]   //http://localhost:[port]/api/movies/query
		                        //http://localhost:[port]/api/movies/query?id=1
		public IActionResult DeleteMovieByQueryStringId(int? id)
		{
			try
			{
				//validations
				if(id == null)
				{
					return BadRequest("Id cannot be null");
				}
				if (id <= 0)
				{
					return BadRequest("Id cannot have a negative value");
				}

				var movieDb = StaticDb.Movies.FirstOrDefault(x => x.Id == id);
				if (movieDb == null)
				{
					return NotFound("Movie was not found");
				}

				//delete
				StaticDb.Movies.Remove(movieDb);
				return StatusCode(StatusCodes.Status204NoContent, "Movie was deleted");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
