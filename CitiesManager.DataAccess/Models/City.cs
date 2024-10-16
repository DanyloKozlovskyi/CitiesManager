using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesManager.DataAccess.Models
{
	public class City
	{
		[Key]
		public Guid Id { get; set; }
		[Required(ErrorMessage = "City Name can't be blank")]
		public string? Name { get; set; }

	}
}
