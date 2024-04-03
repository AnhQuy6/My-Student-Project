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
            if (ModelState.IsValid)
            {
                return BadRequest("Please provide username and password");
            }
            LoginResponseDTO response = new() { Username = model.Username };
            if(model.Username == "Quy" && model.Password == "1212")
            {
                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<String>("JWTSecret"));
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
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
