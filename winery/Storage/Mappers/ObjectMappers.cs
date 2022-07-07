using static Storage.EF.Datastore.WineryContext;
using Contract = Common;

namespace Storage
{
	public static class ObjectMappers
	{
		public static Contract.Establishment MapToWineryContract(this Establishment winery)
		{
			return new Contract.Establishment
			{
				Id = winery.Id,
				Name = winery.Name,
				Region = winery.Region,
				Country = winery.Country
			};
		}

		public static Contract.Wine MapToWineContract(this Wine wine)
		{
			return new Contract.Wine
			{
				Id = wine.Id,
				WineryId = wine.WineryId,
				Name = wine.Name,
				Price = wine.Price,
				Color = (Contract.WineColor)wine.Color,
				Vintage = wine.Vintage,
				IssueDate = wine.IssueDate,
				Note = wine.Note
			};
		}

		public static Establishment MapToWineryPersistence(this Contract.Establishment winery)
		{
			return new Establishment
			{
				Id = winery.Id,
				Name = winery.Name,
				Region = winery.Region,
				Country = winery.Country
			};
		}

		public static Wine MapToWinePersistence(this Contract.Wine wine)
		{
			return new Wine
			{
				Id = wine.Id,
				WineryId = wine.WineryId,
				Name = wine.Name,
				Price = wine.Price,
				Color = (WineColor)wine.Color,
				Vintage = wine.Vintage,
				IssueDate = wine.IssueDate,
				Note = wine.Note
			};
		}
	}
}