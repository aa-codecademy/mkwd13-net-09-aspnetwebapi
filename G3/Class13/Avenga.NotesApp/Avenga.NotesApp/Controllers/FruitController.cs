using Avenga.NotesApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Avenga.NotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitController : ControllerBase
    {
        private readonly IFruitService _FruitService;

        public FruitController(IFruitService fruitService)
        {
            _FruitService = fruitService;
        }

        [HttpGet("fruitName")]
        public async Task<IActionResult> GetFruit(string fruitName)
        {
            try
            {
                var fruitInfo = await _FruitService.GetFruitInfoAsync(fruitName);
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
                var fruits = await _FruitService.GetAllFruitsAsync();
                Log.Information("Successfully retrieved all fruits!");
                return Ok(fruits);
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
                var fruits = await _FruitService.GetFruitsByFamilyAsync(familyName);
                Log.Information("Successfully retrieved the fruit by family name!");
                return Ok(fruits);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("genus/{genusName}")]
        public async Task<IActionResult> GetFruitByGenus(string genusName)
        {
            try
            {
                var fruits = await _FruitService.GetFruitsByGenusAsync(genusName);
                Log.Information("Successfully retrieved the by genus Name!");
                return Ok(fruits);
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
                var fruits = await _FruitService.GetFruitsByOrderAsync(orderName);
                Log.Information("Successfully retrieved the fruit by order name!");
                return Ok(fruits);
            }
            catch (Exception ex)
            {
                Log.Error($"Internal Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
