using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BroadwaysApiInterface.CommonModel;
using BroadwaysApiInterface.Models;
using BroadwaysApiInterface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BroadwaysApiInterface.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegistrationController : ControllerBase
    {
        private readonly IRegisteredService _context;
        private readonly IAuthorization _auth;
        public UserRegistrationController(IRegisteredService context,IAuthorization auth)
        {
            _context = context;
            _auth = auth;
        }
        [HttpGet("Register")]
        public Task Register()
        {
            return _context.GetRegisteredUser();
        }
        [HttpPost("Register")]
        public Task Register(ApplicationUser model)
        {
            return _context.RegisterAsync(model);
        }
        [HttpPost("GetToken")]
        public ActionResult GetToken(User model)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,model.UserName),
                new Claim(ClaimTypes.Role,"admin")
            };
            var jwtResult = _auth.GenerateTokens(model.UserName, claims, DateTime.Now);
            return Ok ( new TokenResultant {
                UserName = model.UserName,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString,
                Role = null
            });

        }
  
    }
    
}