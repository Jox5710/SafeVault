using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserRepository _repo;
    private readonly TokenService _tokenService;

    public AuthController(UserRepository repo, TokenService tokenService)
    {
        _repo = repo;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest("Invalid input.");

        if (dto.Username.Length < 3 || dto.Password.Length < 6)
            return BadRequest("Validation failed.");

        var exists = _repo.GetByUsername(dto.Username);
        if (exists != null) return Conflict("User exists.");

        var user = new User {
            Username = dto.Username,
            PasswordHash = HashPassword(dto.Password),
            Role = dto.Role ?? "User"
        };
        _repo.Create(user);
        return Ok("Registered");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var user = _repo.GetByUsername(dto.Username);
        if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
            return Unauthorized();

        var token = _tokenService.GenerateToken(user.Username, user.Role);
        return Ok(new { token });
    }

    private static string HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        byte[] salt = new byte[16];
        rng.GetBytes(salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);
        var result = new byte[48];
        Buffer.BlockCopy(salt, 0, result, 0, 16);
        Buffer.BlockCopy(hash, 0, result, 16, 32);
        return Convert.ToBase64String(result);
    }

    private static bool VerifyPassword(string password, string stored)
    {
        var bytes = Convert.FromBase64String(stored);
        var salt = new byte[16];
        Buffer.BlockCopy(bytes, 0, salt, 0, 16);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);
        for (int i = 0; i < 32; i++) if (bytes[16 + i] != hash[i]) return false;
        return true;
    }

    public class RegisterDto { public string Username { get; set; } = ""; public string Password { get; set; } = ""; public string? Role { get; set; } }
    public class LoginDto { public string Username { get; set; } = ""; public string Password { get; set; } = ""; }
}
