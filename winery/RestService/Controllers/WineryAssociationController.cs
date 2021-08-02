using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Common.Utils;
using Storage;

namespace RestService.Controllers
{
	[Route("api/wineries/{wineryId}/wines")]
	[ApiController]
	public class WineryAssociationController : ControllerBase
	{
		private readonly IWineryRepository wineryRepository;

		public WineryAssociationController(IWineryRepository wineryRepository)
		{
			this.wineryRepository = wineryRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Get(
			Guid WineryId,
			string token = "",
			int skip = 0,
			int take = 10
			)
		{
			try
			{
				var request = new Request<Guid>
				{
					Token = token,
					Skip = skip,
					Take = take,
					Data = WineryId
				};
				var winesByWinery = await wineryRepository.GetAllWinesFromWineryAsync(request);
				return Ok(winesByWinery);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("{wineId}")]
		public async Task<IActionResult> Get(Guid wineryId, Guid wineId)
		{
			try
			{
				var request = new Request<Tuple<Guid, Guid>>
				{
					Data = new Tuple<Guid, Guid>(wineryId, wineId)
				};

				var winesByWinery = await wineryRepository.GetWineFromWineryByIdAsync(request);
				return Ok(winesByWinery);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}