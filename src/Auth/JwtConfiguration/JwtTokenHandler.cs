using JwtConfiguration.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtConfiguration
{
	public class JwtTokenHandler
	{
		public const string security_key = "";
		private const int token_expiry_time_in_minutes = 20;
		private readonly List<UserAccount> userAccountList;

		public JwtTokenHandler()
		{
			userAccountList = new List<UserAccount>
			{
				new UserAccount { UserName = "admin", Password = "admin123", Role = "Admin" },
				new UserAccount { UserName = "user", Password = "user123", Role = "User" }
			};
		}

		public AuthenticationResponse? GenerateJwtToken(AuthenticationRequest request)
		{
			if(string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
			{
				return null;
			}
			var userAccount = userAccountList.FirstOrDefault(x => x.UserName.Equals(request.UserName, StringComparison.OrdinalIgnoreCase) && x.Password == request.Password);
			if(userAccount == null)
			{
				return null;
			}
			var tokenExpiryTime = DateTime.Now.AddMinutes(token_expiry_time_in_minutes);
			var tokenKey = Encoding.ASCII.GetBytes(security_key);
			var claimsIdentity = new ClaimsIdentity(new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Name, request.UserName),
				new Claim("Role", userAccount.Role)
			});

			var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);
			var securityTokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = claimsIdentity,
				Expires = tokenExpiryTime,
				SigningCredentials = signingCredentials
			};
			var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
			var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
			var token = jwtSecurityTokenHandler.WriteToken(securityToken);

			return new AuthenticationResponse
			{
				UserName = request.UserName,
				JwtToken = token,
				ExpiresIn = (int)tokenExpiryTime.Subtract(DateTime.Now).TotalSeconds
			};
		}
	}
}
