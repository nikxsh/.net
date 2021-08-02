using Common;
using System.Collections.Generic;
using System.Linq;

namespace Storage.EF.Datastore
{
	public class SeedWineryData
	{
		public static HashSet<WineryContext.Establishment> GetWineries()
		{
			return new 
				FileHandler()
				.ReadJsonData<WineryContext.Establishment[]>(@"..\..\Files\wineries.json")
				.ToHashSet();
		}

		public static HashSet<WineryContext.Wine> GetWines()
		{
			return new 
				FileHandler()
				.ReadJsonData<WineryContext.Wine[]>(@"..\..\Files\wines.json")
				.ToHashSet();
		}
	}
}