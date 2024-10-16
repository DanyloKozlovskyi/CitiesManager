using AutoMapper;
using CitiesManager.DataAccess;
using CitiesManager.DataAccess.DTO;
using CitiesManager.DataAccess.Models;
using CitiesManager.WebAPI.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace CitiesService
{
	public class CitiesService : ICitiesService
	{
		private readonly CitiesDbContext dbContext;
		private readonly IMapper mapper;
		public CitiesService(CitiesDbContext db)
		{
			dbContext = db;

			var map = new MapperConfiguration
			(
				mc => mc.AddProfile(new MappingProfile())
			);
			mapper = map.CreateMapper();
		}
		public async Task<City> AddCity(CityCreate? cityCreate)
		{
			if (cityCreate == null)
				throw new ArgumentNullException(nameof(CityCreate));

			City city = mapper.Map<City>(cityCreate);
			await dbContext.AddAsync(city);
			await dbContext.SaveChangesAsync();
			return city;
		}

		public async Task<bool> DeleteCity(Guid? id)
		{
			if (id == null)
				throw new ArgumentNullException(nameof(id));

			City? city = await dbContext.Cities.FirstOrDefaultAsync(x => x.Id == id);
			if (city == null)
				return false;

			dbContext.Cities.Remove(await dbContext.Cities.
				FirstOrDefaultAsync(c => c.Id == id));
			await dbContext.SaveChangesAsync();

			return true;
		}

		public async Task<ICollection<City>> GetCities()
		{
			return await dbContext.Cities.ToListAsync();
		}

		public async Task<City?> GetCityById(Guid? id)
		{
			if (id == null)
				throw new ArgumentNullException(nameof(id));

			return await dbContext.Cities.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<ICollection<City>> GetFilteredCities(Expression<Func<City, bool>> predicate)
		{
			return await dbContext.Cities.Where(predicate).ToListAsync();
		}

		public async Task<City> UpdateCity(City city)
		{
			City? matchingCity = await dbContext.Cities.FirstOrDefaultAsync(x => x.Id == city.Id);

			if (matchingCity == null)
				throw new ArgumentNullException(nameof(city));

			matchingCity.Id = city.Id;
			matchingCity.Name = city.Name;

			await dbContext.SaveChangesAsync();

			return city;
		}
	}
}
