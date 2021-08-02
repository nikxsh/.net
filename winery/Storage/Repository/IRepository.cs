using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.Utils;

namespace Storage
{
	public interface IWineryRepository
	{
		Task<PagedResponse<IEnumerable<Establishment>>> GetAllWineriesAsync(Request request);
		Task<Response<Establishment>> GetWinerybyIdAsync(Request<Guid> request);
		Task<PagedResponse<IEnumerable<Wine>>> GetAllWinesFromWineryAsync(Request<Guid> request);
		Task<Response<Wine>> GetWineFromWineryByIdAsync(Request<Tuple<Guid, Guid>> request);
		Task<Response<Guid>> AddWineryAsync(Establishment request);
		Task<Response<int>> UpdateWineryAsync(Establishment request);
		Task<Response<int>> RemoveWineryAsync(Request<Guid> request);
		Task<Response<bool>> WineryExistsAsync(Request<Guid> request);
	}
	public interface IWineRepository
	{
		Task<PagedResponse<IEnumerable<Wine>>> GetAllWinesAsync(Request request);
		Task<Response<Wine>> GetWineByIdAsync(Request<Guid> request);
		Task<Response<Guid>> AddWineAsync(Wine request);
		Task<Response<int>> UpdateWineAsync(Wine request);
		Task<Response<int>> RemoveWineAsync(Request<Guid> request);
		Task<Response<bool>> WineExistsAsync(Request<Guid> request);
	}
}