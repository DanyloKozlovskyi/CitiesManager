using CitiesManager.DataAccess.DTO;
using CitiesManager.DataAccess.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CitiesManager.WebAPI.Services
{
	public class JwtService : IJwtService
	{
		private readonly IConfiguration configuration;//to read Jwt configuration from appsettings.json
		private readonly string key = "security key value";

		public JwtService(IConfiguration config)
		{
			configuration = config;
		}

		public AuthenticationResponse CreateJwtToken(ApplicationUser user)
		{
			DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:EXPIRATION_MINUTES"]));


			Claim[] claims = new Claim[]
				{
					//JwtRegisteredClaimNames.Sub - user identity
					new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
					//JwtRegisteredClaimNames.Jti - unique id for the token
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					//JwtRegisteredClaimNames.Iat - issued at
					new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
					// further fields are optional
					new Claim(ClaimTypes.NameIdentifier, user.Email),
					new Claim(ClaimTypes.Name, user.PersonName)
				};

			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

			SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			JwtSecurityToken tokenGenerator = new JwtSecurityToken(
				configuration["JWT:Issuer"],
				configuration["JWT:Audience"],
				claims,
				expires: expiration,
				signingCredentials: signingCredentials
				);

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			string token = tokenHandler.WriteToken(tokenGenerator);

			return new AuthenticationResponse() { Token = token, Email = user.Email, PersonName = user.PersonName, Expiration = expiration };
		}

		public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
		{
			throw new NotImplementedException();
		}
	}
}
