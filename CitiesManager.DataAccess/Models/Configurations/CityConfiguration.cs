using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesManager.DataAccess.Models.Configurations
{
	public class CityConfiguration : IEntityTypeConfiguration<City>
	{
		public void Configure(EntityTypeBuilder<City> builder)
		{
			builder.HasKey(x => x.Id);


			builder.HasData(
				new City() { Id = new Guid("EC1A1B75-11D5-4EC6-9D52-3DDAE2EAC040"), Name = "London" },
				new City() { Id = new Guid("239F25AD-6F48-4C01-AFB9-66E39313C534"), Name = "Paris" });
		}
	}
}
