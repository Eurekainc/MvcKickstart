﻿using System.Web.Mvc;
using MvcKickstart.Tests.Utilities;
using MvcKickstart.ViewModels.Account;
using NUnit.Framework;
using Should.Fluent;

namespace MvcKickstart.Tests.Controllers.Account
{
	public class RegisterTests : ControllerTestBase
	{
		[Test]
		public void GivenUnauthenticatedRequest_ReturnsRegisterView()
		{
			var result = Controller.Register("") as ViewResult;
			result.Should().Not.Be.Null();
		}
		[Test]
		public void GivenAuthenticatedRequest_ReturnsHomePage()
		{
			ControllerUtilities.SetupControllerContext(Controller, User);

			var result = Controller.Register("") as RedirectToRouteResult;
			result.Should().Not.Be.Null();
		}

		[Test]
		public void GivenValidModelState_ReturnsConfirmation()
		{
			var model = new Register
			{
				Username = "user1",
				Password = "password1"
			};

			var result = Controller.Register(model) as ViewResult;
			result.Should().Not.Be.Null();
			result.ViewName.Should().Equal("RegisterConfirmation");
			var viewModel = result.Model as Register;
			viewModel.Should().Not.Be.Null();
		}

		[Test]
		public void GivenInvalidModelState_ReturnsRegisterView()
		{
			var model = new Register
			{
				Username = "user1",
				Password = "password1"
			};
			Controller.ModelState.AddModelError("Fake error", "error");

			var result = Controller.Register(model) as ViewResult;
			result.Should().Not.Be.Null();

			result.Model.Should().Be.OfType<Register>();
			var typedModel = result.Model as Register;
			typedModel.Email.Should().Equal(model.Email);
			typedModel.Password.Should().Be.NullOrEmpty();
			typedModel.ReturnUrl.Should().Equal(model.ReturnUrl);
			typedModel.Username.Should().Equal(model.Username);
			result.ViewName.Should().Equal("");

			var modelState = result.ViewData.ModelState;
			modelState.ContainsKey("Fake error").Should().Be.True();
		}

		[Test]
		public void GivenReservedUsername_ReturnsRegisterView()
		{
			var model = new Register
			{
				Username = "admin",
				Password = "password1"
			};

			var result = Controller.Register(model) as ViewResult;
			result.Should().Not.Be.Null();

			result.Model.Should().Be.OfType<Register>();
			result.ViewName.Should().Equal("");
		}

		[Test]
		public void GivenDuplicateUsername_ReturnsRegisterView()
		{
			var model = new Register
			{
				Username = User.Username,
				Password = "password1"
			};

			var result = Controller.Register(model) as ViewResult;
			result.Should().Not.Be.Null();

			result.Model.Should().Be.OfType<Register>();
			result.ViewName.Should().Equal("");
		}

		[Test]
		public void GivenDuplicateEmail_ReturnsRegisterView()
		{
			var model = new Register
			{
				Username = "user1",
				Email = User.Email,
				Password = "password1",
				PasswordConfirm = "password1"
			};

			var result = Controller.Register(model) as ViewResult;
			result.Should().Not.Be.Null();

			result.Model.Should().Be.OfType<Register>();
			result.ViewName.Should().Equal("");
		}

		[Test]
		public void GivenLocalReturnUrl_ReturnsConfirmationWithReturnUrl()
		{
			var model = new Register
			{
				Username = "user1",
				Password = "password1",
				ReturnUrl = "/home/index"
			};

			var result = Controller.Register(model) as ViewResult;
			result.Should().Not.Be.Null();
			result.ViewName.Should().Equal("RegisterConfirmation");
			var viewModel = result.Model as Register;
			viewModel.Should().Not.Be.Null();
			viewModel.ReturnUrl.Should().Equal(model.ReturnUrl);
		}

		[Test]
		public void GivenExternalReturnUrl_ReturnsConfirmation()
		{
			var model = new Register
			{
				Username = "user1",
				Password = "password1",
				ReturnUrl = "http://google.com"
			};

			var result = Controller.Register(model) as ViewResult;
			result.Should().Not.Be.Null();
			result.ViewName.Should().Equal("RegisterConfirmation");
		}
	}
}
