﻿using AutoMapper;
using CitiesManager.DataAccess.DTO;
using CitiesManager.DataAccess.Identity;
using CitiesManager.WebAPI.Services;
using CitiesManager.WebAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[AllowAnonymous]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly RoleManager<ApplicationRole> roleManager;
		private readonly IJwtService jwtService;
		private readonly IMapper mapper;

		public AccountController(UserManager<ApplicationUser> userMng,
			SignInManager<ApplicationUser> signInMng, RoleManager<ApplicationRole> roleMng, IJwtService jwtSvc)
		{
			userManager = userMng;
			signInManager = signInMng;
			roleManager = roleMng;
			jwtService = jwtSvc;

			var map = new MapperConfiguration
			(
				mc => mc.AddProfile(new MappingProfile())
			);
			mapper = map.CreateMapper();
		}

		[HttpPost("register")]
		public async Task<IActionResult> PostRegister(RegisterDTO registerDTO)
		{
			// Validation 
			if (ModelState.IsValid == false)
			{
				string errorMessages = string.Join(" | ", ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage));
				return Problem(errorMessages);
			}

			// Create user
			ApplicationUser user = mapper.Map<ApplicationUser>(registerDTO);

			IdentityResult result = await userManager.CreateAsync(user, registerDTO.Password);

			if (result.Succeeded == true)
			{
				// sign-in
				// isPersister: false - must be deleted automatically when the browser is closed
				await signInManager.SignInAsync(user, isPersistent: false);

				var authenticationResponse = jwtService.CreateJwtToken(user);

				return Ok(authenticationResponse);
			}

			string errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
			return Problem(errorMessage);


		}

		[HttpGet]
		public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
		{
			ApplicationUser? user = await userManager.FindByEmailAsync(email);

			if (user == null)
			{
				return Ok(true);
			}
			return Ok(false);
		}

		[HttpPost("login")]
		public async Task<IActionResult> PostLogin(LoginDTO loginDTO)
		{
			// Validation 
			if (ModelState.IsValid == false)
			{
				string errorMessages = string.Join(" | ", ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage));
				return Problem(errorMessages);
			}

			var result = await signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

			if (result.Succeeded == true)
			{
				ApplicationUser? user = await userManager.FindByEmailAsync(loginDTO.Email);

				if (user == null)
					return NoContent();

				await signInManager.SignInAsync(user, isPersistent: false);

				var authenticationResponse = jwtService.CreateJwtToken(user);

				return Ok(authenticationResponse);
			}
			return Problem("Invalid email or password");
		}

		[HttpGet("logout")]
		public async Task<IActionResult> GetLogout()
		{
			await signInManager.SignOutAsync();

			return NoContent();
		}
	}
}
