using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Utils;
using Storage.Datastore;

namespace Storage
{
	public class WineryRepository : IWineryRepository
	{
		public IWineryDataStore DataStore;

		public WineryRepository(IWineryDataStore dataStore)
		{
			DataStore = dataStore;
		}

		public async Task<PagedResponse<IEnumerable<Establishment>>> GetAllWineriesAsync(Request request)
		{
			try
			{
				var response = new PagedResponse<IEnumerable<Establishment>>();

				var wineries = await DataStore.GetAllWineriesAsync();

				response.Total = wineries.Count();
				wineries = wineries.SearchWineries(request.Token);
				wineries = wineries.FilterWineries(request.Filters);
				wineries = wineries.SortWineries(request.Sort);
				wineries = wineries.Skip(request.Skip).Take(request.Take);

				response.Result = wineries.Select(x => x.MapToWineryContract());
				return response;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<Response<Establishment>> GetWinerybyIdAsync(Request<Guid> request)
		{
			try
			{
				var response = new Response<Establishment>();

				var winery = await DataStore.GetWineryByIdAsync(request.Data);
				response.Result = winery.MapToWineryContract();

				return response;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<PagedResponse<IEnumerable<Wine>>> GetAllWinesFromWineryAsync(Request<Guid> request)
		{
			try
			{
				var response = new PagedResponse<IEnumerable<Wine>>();

				var wines = await DataStore.GetAllWinesFromWineryAsync(request.Data);

				response.Total = wines.Count();
				wines = wines.SearchWines(request.Token);
				wines = wines.FilterWines(request.Filters);
				wines = wines.SortWines(request.Sort);
				wines = wines.Skip(request.Skip).Take(request.Take);

				response.Result = wines.Select(x => x.MapToWineContract());
				return response;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<Response<Wine>> GetWineFromWineryByIdAsync(Request<Tuple<Guid, Guid>> request)
		{
			try
			{
				var response = new Response<Wine>();

				var wine = await DataStore.GetWineFromWineryByIdAsync(request.Data.Item1, request.Data.Item2);
				response.Result = wine.MapToWineContract();

				return response;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<Response<Guid>> AddWineryAsync(Establishment winery)
		{
			try
			{
				var response = new Response<Guid>
				{
					Result = await DataStore.AddWineryAsync(winery.MapToWineryPersistence())
				};
				return response;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<Response<int>> UpdateWineryAsync(Establishment winery)
		{
			try
			{
				var response = new Response<int>
				{
					Result = await DataStore.UpdateWineryAsync(winery.MapToWineryPersistence())
				};
				return response;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<Response<int>> RemoveWineryAsync(Request<Guid> request)
		{
			try
			{
				var response = new Response<int>
				{
					Result = await DataStore.RemoveWineryAsync(request.Data)
				};

				return response;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<Response<bool>> WineryExistsAsync(Request<Guid> request)
		{
			try
			{
				return new Response<bool>
				{
					Result = await DataStore.WineryExistsAsync(request.Data)
				};
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}