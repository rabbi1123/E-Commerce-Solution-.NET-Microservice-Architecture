using JwtConfiguration;
using JwtConfiguration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly JwtTokenHandler jwtTokenHandler;

		public AccountController(JwtTokenHandler jwtTokenHandler)
		{
			this.jwtTokenHandler = jwtTokenHandler;
		}

		public ActionResult<AuthenticationResponse?> Authenticate(AuthenticationRequest request)
		{
			var authenticationResponse = jwtTokenHandler.GenerateJwtToken(request);
			if (authenticationResponse == null)
			{
				return Unauthorized();
			}
			else
			{
				return authenticationResponse;
			}
		}
	}
}
