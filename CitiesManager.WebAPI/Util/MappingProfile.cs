using AutoMapper;
using CitiesManager.DataAccess.DTO;
using CitiesManager.DataAccess.Identity;
using CitiesManager.DataAccess.Models;

namespace CitiesManager.WebAPI.Util
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<City, CityCreate>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
			CreateMap<CityCreate, City>().ForMember(dest => dest.Id, opt => Guid.NewGuid()).ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

			CreateMap<RegisterDTO, ApplicationUser>()
				.ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.PersonName))
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
				.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
		}
	}
}
