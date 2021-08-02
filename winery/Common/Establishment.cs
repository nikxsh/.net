﻿using System;

namespace Common
{
	public abstract class Base
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}

	public class Establishment : Base
	{
		public string Region { get; set; }
		public string Country { get; set; }
	}

	public class Wine : Base
	{
		public Guid WineryId { get; set; }
		public WineColor Color { get; set; }
		public string Vintage { get; set; }
		public decimal Price { get; set; }
		public DateTime IssueDate { get; set; }
		public string Note { get; set; }
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