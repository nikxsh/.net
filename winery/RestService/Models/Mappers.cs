using System;
using RestService.Models;
using Contract = Common;

namespace RestService.Mappers
{
	public static class Mappers
	{
		public static Contract.Establishment MapToWineryContract(this EstablishmentDTO winery)
		{
			return new Contract.Establishment
			{
				Id = winery.Id,
				Name = winery.Name,
				Region = winery.Region,
				Country = winery.Country
			};
		}

		public static Contract.Wine MapToWineContract(this WineDTO wine)
		{
			return new Contract.Wine
			{
				Id = wine.Id,
				WineryId = wine.WineryId,
				Name = wine.Name,
				Price = wine.Price,
				Color = (Contract.WineColor)wine.Color,
				Vintage = wine.Vintage,
				IssueDate = DateTime.Parse(wine.IssueDate),
				Note = wine.Note
			};
		}

		public static EstablishmentDTO MapToWineryPersistence(this Contract.Establishment winery)
		{
			return new EstablishmentDTO
			{
				Id = winery.Id,
				Name = winery.Name,
				Region = winery.Region,
				Country = winery.Country
			};
		}

		public static WineDTO MapToWinePersistence(this Contract.Wine wine)
		{
			return new WineDTO
			{
				Id = wine.Id,
				WineryId = wine.WineryId,
				Name = wine.Name,
				Price = wine.Price,
				Color = (WineColor)wine.Color,
				Vintage = wine.Vintage,
				IssueDate = wine.IssueDate.ToString(),
				Note = wine.Note
			};
		}
	}
}