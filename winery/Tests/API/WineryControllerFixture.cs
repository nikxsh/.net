using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestService.Controllers;
using Common;
using Common.Utils;
using Storage;

namespace Tests
{
	[TestFixture]
	public class WineryControllerFixture
	{
		private readonly IWineryRepository _wineryRepository;
		private readonly WineryController _controller;


		public WineryControllerFixture()
		{
			_wineryRepository = Substitute.For<IWineryRepository>();

			_controller = new WineryController(_wineryRepository);
		}

		[TestCase]
		public void Get_should_return_ok()
		{
			// Arrange
			_wineryRepository
				.GetAllWineriesAsync(Arg.Any<Request>())
				.Returns(Task.FromResult(new PagedResponse<IEnumerable<Establishment>>())
				);

			// Act
			var okResult = _controller.Get().Result;

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(okResult);
		}

		[TestCase]
		public void Get_should_return_all_wineries()
		{
			// Arrange
			_wineryRepository
				.GetAllWineriesAsync(Arg.Any<Request>())
				.Returns(Task.FromResult(
					new PagedResponse<IEnumerable<Establishment>>
					{
						Result = new List<Establishment> { new Establishment(), new Establishment() },
						Total = 2
					})
				);

			// Act
			var okResult = _controller.Get().Result as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			var resultSet = okResult.Value as PagedResponse<IEnumerable<Establishment>>;
			Assert.NotNull(resultSet);
			Assert.AreEqual(2, resultSet.Result.Count());
			Assert.AreEqual(2, resultSet.Total);
		}

		[TestCase]
		public void Get_by_id_should_return_ok()
		{
			// Arrange
			_wineryRepository
				.GetWinerybyIdAsync(Arg.Any<Request<Guid>>())
				.Returns(Task.FromResult(
					new Response<Establishment>
					{
						Result = new Establishment()
					})
				);

			// Act
			var okResult = _controller.Get(Guid.Empty).Result;

			// Assert
			Assert.IsInstanceOf<OkObjectResult>(okResult);
		}

		[TestCase]
		public void Get_by_id_should_return_not_found()
		{
			// Arrange
			_wineryRepository
				.GetWinerybyIdAsync(Arg.Any<Request<Guid>>())
				.Returns(Task.FromResult(
					new Response<Establishment>
					{
						Result = null
					})
				);

			// Act
			var okResult = _controller.Get(Guid.Empty).Result;

			// Assert
			Assert.IsInstanceOf<NotFoundObjectResult>(okResult);
		}

		[TestCase]
		public void Get_by_id_should_return_winery()
		{
			// Arrange
			_wineryRepository
				.GetWinerybyIdAsync(Arg.Any<Request<Guid>>())
				.Returns(Task.FromResult(
					new Response<Establishment>
					{
						Result = new Establishment()
					})
				);

			// Act
			var okResult = _controller.Get(Guid.Empty).Result as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			var resultSet = okResult.Value as Response<Establishment>;
			Assert.NotNull(resultSet);
		}
	}
}