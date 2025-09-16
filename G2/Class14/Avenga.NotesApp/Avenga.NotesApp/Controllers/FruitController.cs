using Avenga.NotesApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Avenga.NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitController : ControllerBase
    {
        private readonly IFruitService _fruitService;

        public FruitController(IFruitService fruitService)
        {
            _fruitService = fruitService;
        }

        [HttpGet("fruitName")]
        public async Task<IActionResult> GetFruit(string fruitName)
        {
            try
            {
                var fruitInfo = await _fruitService.GetFruitInfoAsync(fruitName);
                Log.Information("Successfully retrieved the fruit with its info!");
                return Ok(fruitInfo);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFruits()
        {
            try
            {
                var fruitInfo = await _fruitService.GetAllFruitsAsync();
                Log.Information("Successfully retrieved all fruits!");
                return Ok(fruitInfo);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("family/{familyName}")]
        public async Task<IActionResult> GetFruitsByFamily(string familyName)
        {
            try
            {
                var fruitInfo = await _fruitService.GetFruitsByFamilyAsync(familyName);
                Log.Information("Successfully retrieved fruits by family!");
                return Ok(fruitInfo);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("grnus/{genusName}")]
        public async Task<IActionResult> GetFruitsByGenus(string genusName)
        {
            try
            {
                var fruitInfo = await _fruitService.GetFruitsByGenusAsync(genusName);
                Log.Information("Successfully retrieved all fruits by genus!");
                return Ok(fruitInfo);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("order/{orderName}")]
        public async Task<IActionResult> GetFruitsByOrder(string orderName)
        {
            try
            {
                var fruitInfo = await _fruitService.GetFruitsByOrderAsync(orderName);
                Log.Information("Successfully retrieved all fruits by order!");
                return Ok(fruitInfo);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
