using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using RestService.Mappers;
using RestService.Models;
using Common.Utils;
using Storage;

namespace RestService.Controllers
{
	[Route("api")]
	[ApiController]
	public class WineController : ControllerBase
	{
		private readonly IWineRepository wineRepository;

		public WineController(IWineRepository wineRepository)
		{
			this.wineRepository = wineRepository;
		}

		[HttpPost("wines")]
		public async Task<IActionResult> Get(Request request)
		{
			try
			{
				var allWines = await wineRepository.GetAllWinesAsync(request);

				return Ok(allWines);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("wines/{wineId:Guid}")]
		public async Task<IActionResult> Get(Guid wineId)
		{
			try
			{
				var request = new Request<Guid>
				{
					Data = wineId
				};

				var existingWine = await wineRepository.WineExistsAsync(request);

				if (!existingWine.Result)
					return NotFound($"Wine with Id {wineId} does not exists.");

				var wine = await wineRepository.GetWineByIdAsync(request);
				return Ok(wine);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost("wine")]
		public async Task<IActionResult> Post(WineDTO request)
		{
			try
			{
				var response = await wineRepository.AddWineAsync(request.MapToWineContract());

				if (response.Result != Guid.Empty)
				{
					request.Id = response.Result;
					return Created($"api/wines/{request.Id}", request);
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Add Wine!");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut("wine")]
		public async Task<IActionResult> Put(WineDTO request)
		{
			try
			{
				var existingWine = await wineRepository.WineExistsAsync(
					new Request<Guid>
					{
						Data = request.Id
					});

				if (!existingWine.Result)
					return NotFound($"Wine with Id {request.Id} does not exists.");

				var response = await wineRepository.UpdateWineAsync(request.MapToWineContract());

				if (response.Result > 0)
					return Ok(request);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Update Wine!");

			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpDelete("wine/{wineId:Guid}")]
		public async Task<IActionResult> Delete(Guid wineId)
		{
			try
			{
				var existingWine = await wineRepository.GetWineByIdAsync(
					new Request<Guid>
					{
						Data = wineId
					});

				if (existingWine.Result == null)
					return NotFound($"Wine with Id {wineId} does not exists.");

				var response = await wineRepository.RemoveWineAsync(new Request<Guid> { Data = wineId });

				if (response.Result > 0)
					return Ok(wineId);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Failed to remove Wine!");
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}