using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentProject.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "LoginForLocalUsers", Roles = "Superadmin,Admin")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpPost]
        public ActionResult Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide username and password");
            }
            LoginResponseDTO response = new() { Username = model.Username };
            string issuer = string.Empty;
            string audience = string.Empty;
            byte[] key = null;
            if (model.Policy == "Local")
            {
                issuer = _configuration.GetValue<String>("LocalIssuer");
                audience = _configuration.GetValue<String>("LocalAudience");
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<String>("JWTSecretForLocal"));
            }
            else if (model.Policy == "Microsoft")
            {
                issuer = _configuration.GetValue<String>("MicrosoftIssuer");
                audience = _configuration.GetValue<String>("MicrosoftAudience");
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<String>("JWTSecretForMicrosoft"));
            }
            else if (model.Policy == "Google")
            {
                issuer = _configuration.GetValue<String>("GoogleIssuer");
                audience = _configuration.GetValue<String>("GoogleAudience");
                key = Encoding.ASCII.GetBytes(_configuration.GetValue<String>("JWTSecretForGoogle"));
            }
            if (model.Username == "quygagay" && model.Password == "Anhquy123")
            { 
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Issuer = issuer,
                    Audience = audience,
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        //Username
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim(ClaimTypes.Role, "Admin")
                    }),
                    Expires = DateTime.Now.AddHours(4),
                    SigningCredentials = new (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                response.Token = tokenHandler.WriteToken(token);
            }
            else
            {
                return BadRequest("Invalid username and password");
            }
            return Ok(response);
        }
    }
}
