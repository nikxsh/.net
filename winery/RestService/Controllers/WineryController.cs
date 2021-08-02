using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using RestService.Mappers;
using RestService.Models;
using Common.Utils;
using Storage;

namespace RestService.Controllers
{
	[Route("api/wineries")]
	[ApiController]
	public class WineryController : ControllerBase
	{
		private readonly IWineryRepository wineryRepository;

		public WineryController(IWineryRepository wineryRepository)
		{
			this.wineryRepository = wineryRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Get(
			string token = "",
			int skip = 0,
			int take = 10
			)
		{
			var request = new Request
			{
				Token = token,
				Skip = skip,
				Take = take
			};

			try
			{
				var wineries = await wineryRepository.GetAllWineriesAsync(request);
				return Ok(wineries);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}


		[HttpGet("{wineryId:Guid}")]
		public async Task<IActionResult> Get(Guid wineryId)
		{
			try
			{
				var request = new Request<Guid>
				{
					Data = wineryId
				};

				var wineryExists = await wineryRepository.WineryExistsAsync(request);

				if (!wineryExists.Result)
					return NotFound($"Winery with Id {wineryId} does not exists.");

				var winery = await wineryRepository.GetWinerybyIdAsync(request);

				return Ok(winery);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post(EstablishmentDTO request)
		{
			try
			{
				var response = await wineryRepository.AddWineryAsync(request.MapToWineryContract());

				if (response.Result != Guid.Empty)
				{
					request.Id = response.Result;
					return Created($"api/winery/{request.Id}", request);
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Add Winery!");
			}
			catch (Exception ex)
			{
				if (ex is FormatException)
					return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut]
		public async Task<IActionResult> Put(EstablishmentDTO request)
		{
			try
			{
				var winery = await wineryRepository.WineryExistsAsync(
					new Request<Guid>
					{
						Data = request.Id
					});

				if (!winery.Result)
					return NotFound($"Winery with Id {request.Id} does not exists.");

				var response = await wineryRepository.UpdateWineryAsync(request.MapToWineryContract());

				if (response.Result > 0)
					return Ok(request);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Failed to Update Winery!");
			}
			catch (Exception ex)
			{
				if (ex is FormatException)
					return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPatch("{wineryId}")]
		public async Task<IActionResult> Patch(Guid wineryId, [FromBody] JsonPatchDocument<EstablishmentDTO> wineryPatch)
		{
			try
			{
				var winery = await wineryRepository.WineryExistsAsync(
					new Request<Guid>
					{
						Data = wineryId
					});

				if (!winery.Result)
					return NotFound($"Winery with Id {wineryId} does not exists.");

				EstablishmentDTO wineryDTO = new EstablishmentDTO();
				wineryPatch.ApplyTo(wineryDTO);

				return Ok(wineryDTO);
			}
			catch (Exception ex)
			{
				if (ex is FormatException)
					return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpDelete("{wineryId}")]
		public async Task<IActionResult> Delete(Guid wineryId)
		{
			try
			{
				var winery = await wineryRepository.WineryExistsAsync(
					new Request<Guid>
					{
						Data = wineryId
					});

				if (!winery.Result)
					return NotFound($"Winery with Id {wineryId} does not exists.");

				var response = await wineryRepository.RemoveWineryAsync(new Request<Guid> { Data = wineryId });

				if (response.Result > 0)
					return Ok(wineryId);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Failed to remove Winery!");
			}
			catch (Exception ex)
			{
				if (ex is FormatException)
					return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
				else
					return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}