using System;
using System.Linq;
using System.Threading.Tasks;
using static Storage.EF.Datastore.WineryContext;

namespace Storage.Datastore
{
	public interface IWineryDataStore
	{
		Task<IQueryable<Establishment>> GetAllWineriesAsync();
		Task<Establishment> GetWineryByIdAsync(Guid id);
		Task<IQueryable<Wine>> GetAllWinesFromWineryAsync(Guid wineryId);
		Task<Wine> GetWineFromWineryByIdAsync(Guid wineryId, Guid wineId);
		Task<Guid> AddWineryAsync(Establishment winery);
		Task<int> UpdateWineryAsync(Establishment winery);
		Task<int> RemoveWineryAsync(Guid wineryId);
		Task<bool> WineryExistsAsync(Guid wineryId);
		Task<bool> WineryExistsAsync(string wineryName);
	}

	public interface IWineDataStore
	{
		Task<IQueryable<Wine>> GetAllWinesAsync();
		Task<Wine> GetWineByIdAsync(Guid wineId);
		Task<Guid> AddWineAsync(Wine wine);
		Task<int> UpdateWineAsync(Wine wine);
		Task<int> RemoveWineAsync(Guid wineId);
		Task<bool> WineExistsAsync(Guid wineId);
		Task<bool> WineExistsAsync(string wineName);
	}
}