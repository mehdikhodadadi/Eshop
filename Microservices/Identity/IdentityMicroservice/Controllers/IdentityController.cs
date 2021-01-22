using IdentityModel;
using IdentityRepository;
using Microsoft.AspNetCore.Mvc;
using Middleware;
using System;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IEncryptor _encryptor;
        private readonly IJwtBuilder _jwtBuilder;
        private readonly IUserRepository _userRepository;

        public IdentityController(IUserRepository userRepository, IJwtBuilder jwtBuilder, IEncryptor encryptor)
        {
            _userRepository = userRepository;
            _jwtBuilder = jwtBuilder;
            _encryptor = encryptor;
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] User user, [FromQuery(Name = "d")] string destination = "client")
        {
            var _user = _userRepository.GetUser(user.Email);

            if (_user == null)
            {
                return NotFound("User not found.");
            }

            if (destination == "admin" && !_user.IsAdmin)
            {
                return BadRequest("Could not authenticate user.");
            }

            var isValid = _user.ValidatePassword(user.Password, _encryptor);

            if (!isValid)
            {
                return BadRequest("Could not authenticate user.");
            }

            var token = _jwtBuilder.GetToken(_user.Id);

            return Ok(token);
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] User user)
        {
            var _user = _userRepository.GetUser(user.Email);

            if (_user != null)
            {
                return BadRequest("User already exists.");
            }

            user.SetPassword(user.Password, _encryptor);
            _userRepository.InsertUser(user);

            return Ok();
        }

        [HttpGet("validate")]
        public ActionResult<Guid> Validate([FromQuery(Name = "email")] string email, [FromQuery(Name = "token")] string token)
        {
            var _user = _userRepository.GetUser(email);

            if (_user == null)
            {
                return NotFound("User not found.");
            }

            var userId = _jwtBuilder.ValidateToken(token);

            if (userId != _user.Id)
            {
                return BadRequest("Invalid token.");
            }

            return Ok(userId);
        }
    }
}