using AutoMapper;
using CitiesManager.DataAccess;
using CitiesManager.DataAccess.DTO;
using CitiesManager.DataAccess.Models;
using CitiesManager.WebAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// it is necessary to have default constructor for dbContext to scaffold
//use enable cors attribute to enable custom policies of corses
//and ng serve --port=3200

namespace CitiesManager.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[AllowAnonymous]
	//[EnableCors("3200Client")]
	public class CityController : ControllerBase
	{
		private readonly ICitiesService citiesService;
		private readonly IMapper mapper;
		public CityController(ICitiesService service)
		{
			citiesService = service;

			var map = new MapperConfiguration
			(
				mc => mc.AddProfile(new MappingProfile())
			);
			mapper = map.CreateMapper();
		}

		// GET: api/City
		[HttpGet]
		public async Task<IActionResult> GetCities()
		{
			var result = await citiesService.GetCities().ConfigureAwait(false);
			return Ok(result);
		}

		// GET: api/City/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetCity(Guid id)
		{
			var city = await citiesService.GetCityById(id).ConfigureAwait(false);

			if (city == null)
			{
				return NotFound();
			}

			return Ok(city);
		}

		// PUT: api/City/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCity(Guid id, City city)
		{
			if (id != city.Id)
			{
				return BadRequest();
			}

			try
			{
				await citiesService.UpdateCity(city).ConfigureAwait(false);
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await CityExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/City
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<City>> PostCity(City city)
		{
			await citiesService.AddCity(mapper.Map<CityCreate>(city)).ConfigureAwait(false);

			return CreatedAtAction("GetCity", new { id = city.Id }, city);
		}

		// DELETE: api/City/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCity(Guid id)
		{
			var city = await citiesService.DeleteCity(id).ConfigureAwait(false);
			if (city == false)
			{
				return NotFound();
			}

			return NoContent();
		}

		private async Task<bool> CityExists(Guid id)
		{
			return await citiesService.CityExists(id).ConfigureAwait(false);
		}
	}
}
