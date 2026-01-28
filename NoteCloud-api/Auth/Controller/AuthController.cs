using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteCloud_api.Auth.Dto;
using NoteCloud_api.Auth.Services;
using NoteCloud_api.Users.Dto;
using NoteCloud_api.Users.Exceptions;
using NoteCloud_api.Users.Service;

namespace NoteCloud_api.Auth.Controllers
{
    public record LoginResponse(string Token, DateTime ExpiresAt, string Role);

    public record LoginRequest
    {
        [Required]
        public string Email { get; init; } = string.Empty;

        [Required]
        public string Password { get; init; } = string.Empty;
    }

    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthenticator _authenticator;
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly ICommandServiceUser _commandUser;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserAuthenticator authenticator,
            IJwtTokenGenerator tokenGenerator,
            ICommandServiceUser commandUser,
            ILogger<AuthController> logger)
        {
            _authenticator = authenticator ?? throw new ArgumentNullException(nameof(authenticator));
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
            _commandUser = commandUser ?? throw new ArgumentNullException(nameof(commandUser));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest req)
        {
            if (req is null)
                return BadRequest(new { message = "Invalid request" });

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var created = await _commandUser.CreateUser(new UserRequest
                {
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    Email = req.Email,
                    Password = req.Password,
                    Role = "User"
                });

                return Created("", created);
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
                return Unauthorized("Invalid credentials");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var user = await _authenticator.AuthenticateAsync(request.Email, request.Password, cancellationToken);
            if (user is null)
            {
                _logger.LogInformation("Invalid login attempt for {Email}", request.Email);
                return Unauthorized("Invalid credentials");
            }

            var generated = _tokenGenerator.GenerateToken(user);
            return Ok(new LoginResponse(generated.Token, generated.ExpiresAt, user.Role));
        }
    }
}
