using CitiesManager.DataAccess;
using CitiesManager.DataAccess.DTO;
using CitiesManager.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public interface ICitiesService
	{
		Task<City> AddCity(CityCreate? city);
		Task<ICollection<City>> GetCities();
		Task<City?> GetCityById(Guid? id);
		Task<ICollection<City>> GetFilteredCities(Expression<Func<City, bool>> predicate);
		Task<City> UpdateCity(City city);
		Task<bool> DeleteCity(Guid? id);
		Task<bool> CityExists(Guid id);



	}
}
