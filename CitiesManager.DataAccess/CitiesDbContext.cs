using CitiesManager.DataAccess.Identity;
using CitiesManager.DataAccess.Models;
using CitiesManager.DataAccess.Models.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesManager.DataAccess
{
	public class CitiesDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
	{
		public DbSet<City> Cities { get; set; }
		public CitiesDbContext(DbContextOptions options) : base(options)
		{

		}
		public CitiesDbContext()
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new CityConfiguration());

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			// set connection string
			optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=City;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
		}

	}
}
