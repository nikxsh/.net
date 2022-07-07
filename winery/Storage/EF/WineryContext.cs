﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Storage.EF.Datastore
{
	public class WineryContext : DbContext
	{
		public DbSet<Establishment> Wineries { get; set; }
		public DbSet<Wine> Wines { get; set; }

		public WineryContext(DbContextOptions<WineryContext> dbContextOptions) : base(dbContextOptions)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.EnableSensitiveDataLogging();
			optionsBuilder.UseSqlServer(@"Server=(local);Database=Test;Integrated Security=True", b => b.MigrationsAssembly("WineryStore.API"));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Establishment>()
				.ToTable("Wineries");

			modelBuilder.Entity<Establishment>()
				.HasKey(w => w.Id);

			modelBuilder.Entity<Wine>()
				.ToTable("Wines");

			modelBuilder.Entity<Wine>()
				.HasKey(w => w.Id);

			modelBuilder.Entity<Wine>()
				.HasOne(wine => wine.WineryRelation)
				.WithMany(winery => winery.Wines)
				.HasForeignKey(w => w.WineryId);

			modelBuilder.Entity<Wine>()
				.Property(x => x.Color)
				.HasConversion<int>();

			//Seed Mock Data
			modelBuilder.Entity<Establishment>().HasData(SeedWineryData.GetWineries());
			modelBuilder.Entity<Wine>().HasData(SeedWineryData.GetWines());

			//https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/index
			//1. Install-Package Microsoft.EntityFrameworkCore.Design
			//2. dotnet ef migrations add InitialWineryCreate -s ..\RestService  (-c contextName) /// Add-Migration InitialWineShopCreate 
			//3. dotnet ef database update -s ..\RestService /// Update-Database
			//4. dotnet ef migrations remove -s ..\RestService

			base.OnModelCreating(modelBuilder);
		}

		public abstract class Base
		{
			public Guid Id { get; set; }
			public string Name { get; set; }

		}

		public class Establishment : Base
		{
			public string Region { get; set; }
			public string Country { get; set; }

			//one-to-many relationship
			public List<Wine> Wines { get; set; }
		}

		public class Wine : Base
		{
			public WineColor Color { get; set; }
			public string Vintage { get; set; }
			public decimal Price { get; set; }
			public DateTime IssueDate { get; set; }
			public string Note { get; set; }

			//It is recommended to have a foreign key property defined in the dependent entity class, it is not required. 
			//If no foreign key property is found, a shadow foreign key property will be introduced with the name 
			// <navigation property name><principal key property name>
			public Guid WineryId { get; set; }
			//[ForeignKey("WineryForeignKey")]
			public Establishment WineryRelation { get; set; }
		}

		public enum WineColor
		{
			Blush,
			Champagne,
			Dessert,
			Red,
			Rose,
			Sparkling,
			White
		}
	}
}