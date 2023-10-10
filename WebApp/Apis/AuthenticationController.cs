using Application.Services.Role;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebApp.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration _config;
        private readonly SignInManager<Users> _signInManager;
        private readonly UserManager<Users> _userManager;
        private readonly IRoleServices _roleServices;
        public AuthenticationController(IConfiguration config, UserManager<Users> _userManager, SignInManager<Users> _signInManager, IRoleServices roleServices)
        {
            _config = config;
            this._signInManager = _signInManager;
            this._userManager = _userManager;
            _roleServices = roleServices;
        }

        [HttpPost]
        [Route("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser(LoginVm model)
        {
            if (String.IsNullOrEmpty(model.username))
                return Ok("UserName/Email is required.");
            else if (String.IsNullOrEmpty(model.password))
                return Ok("Password is required.");
            else
            {
                if (model.username =="abc" && model.password == "abc")
                {
                    return Ok(GenerateJSONWebToken(model.username));
                }
                else
                    return Ok("Invalid User.");
            }
        }

        private string GenerateJSONWebToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddDays(2),
              signingCredentials: credentials);
            //var jsonu = new { id = userInfo.Id,firstname=userInfo.FirstName,lsastname=userInfo.LastName };
            //token.Payload["user"] = jsonu;
            //token.Payload["userid"] = userInfo.Id;
            //token.Payload["firstname"] = userInfo.FirstName;
            //token.Payload["middlename"] = userInfo.MiddleName;
            //token.Payload["lastname"] = userInfo.LastName;
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    public class LoginVm
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
