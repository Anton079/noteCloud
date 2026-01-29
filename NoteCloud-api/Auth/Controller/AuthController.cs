using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteCloud_api.Auth.Dto;
using NoteCloud_api.Auth.Services;
using NoteCloud_api.Users.Dto;
using NoteCloud_api.Users.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace NoteCloud_api.Auth.Controllers
{
    public record LoginResponse(string Token, DateTime ExpiresAt, string Role);

    public record LoginRequest
    {
        [SwaggerSchema(Description = "Example: admin@notecloud.local")]
        public string Email { get; init; } = string.Empty;

        [SwaggerSchema(Description = "Example: Admin123!")]
        public string Password { get; init; } = string.Empty;
    }

    [ApiController]
    [Route("api/v1/auth")]
    [SwaggerTag("Authentication and token issuance")]
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
        [SwaggerOperation(
            Summary = "Register a new user",
            Description = "Creates a new account with role `User`. Returns the created user."
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "User created", typeof(UserResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Email already exists")]
        public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest req)
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

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Authenticate and get JWT",
            Description = "Returns a JWT token for the supplied credentials."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Token issued", typeof(LoginResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
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

