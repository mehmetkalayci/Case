using Case.Server.Data;
using Case.Server.Data.Models;
using Case.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Case.Server.Controllers;



[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public AuthController(IConfiguration configuration, AppDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
        {
            return Problem("Lütfen, zorunlu alanları doldurun.");
        }

        var userExists = await _context.Users.Where(x => x.Email == registerViewModel.Email).FirstOrDefaultAsync();

        if (userExists != null)
        {
            return Problem($"{registerViewModel.Email} ile kayıt kayıtlı bir kullanıcı var.");
        }

        User newUser = new User()
        {
            FullName = registerViewModel.FullName,
            Email = registerViewModel.Email,
            Password = registerViewModel.Password,
            Role = AuthRoles.Kullanici
        };

        await _context.AddAsync(newUser);
        int result = await _context.SaveChangesAsync();

        if (result > 0)
        {
            return Ok("Kaydınız alınmıştır, eposta adresiniz ile giriş yapabilirsiniz.");
        }
        else
        {
            return Problem("Kayıt sırasında hata oluştu!");
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return Problem($"Bilgilerinizi kontrol edip tekrar gönderin.");
        }


        var user = await _context.Users.Where(x => x.Email == loginRequest.Email && x.Password == loginRequest.Password && x.DeletedAt == null).FirstOrDefaultAsync();

        if (user == null)
        {
            return Problem("Kullanıcı bilgilerini kontrol edin!");
        }
        else
        {
            var tokenValue = GenerateToken(new LoggedInUserVM
            {
                UserId = user.UserId,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role
            });

            return Ok(tokenValue);
        }
    }

    [HttpGet("profile")]
    [Authorize()]
    public async Task<IActionResult> Profile()
    {
        LoggedInUserVM? loggedInUser = await GetCurrentUser();

        if (loggedInUser != null)
        {
            return Ok(loggedInUser);
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpPost("delete-account")]
    [Authorize(AuthRoles.Admin)]
    public async Task<IActionResult> DeleteAccount()
    {
        LoggedInUserVM? loggedInUser = await GetCurrentUser();

        if (loggedInUser != null)
        {
            User? currentUser = await _context.Users.Where(x => x.UserId == loggedInUser.UserId).FirstOrDefaultAsync();

            if (currentUser == null)
            {
                return Problem("Kullanıcı bulunamadı!");
            }
            else
            {
                currentUser.DeletedAt = DateTime.UtcNow;

                int result = _context.SaveChanges();

                if (result > 0)
                {
                    return Ok("Hesabınız silinmiştir.");
                }
                else
                {
                    return Ok("Hesap silinemedi!");
                }
            }
        }
        else
        {
            return Unauthorized();
        }
    }

    private string GenerateToken(LoggedInUserVM loggedInUser)
    {
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, loggedInUser.UserId.ToString()),
            new Claim(ClaimTypes.Email, loggedInUser.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, loggedInUser.Role)
        };

        var jwtSecretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            expires: DateTime.UtcNow.AddDays(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(jwtSecretKey, SecurityAlgorithms.HmacSha256)
            );

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return jwtToken;
    }

    private async Task<LoggedInUserVM?> GetCurrentUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        if (identity != null)
        {
            var claims = identity.Claims;

            string userId = claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            if (!string.IsNullOrEmpty(userId))
            {
                // get user from db according to the login rules
                var currentUser = await _context.Users.Where(x => x.DeletedAt == null && x.UserId == int.Parse(userId)).FirstOrDefaultAsync();

                if (currentUser != null)
                {
                    return new LoggedInUserVM()
                    {
                        UserId = currentUser.UserId,
                        FullName = currentUser.FullName,
                        Email = currentUser.Email,
                        Role = currentUser.Role,
                    };
                }
            }
        }
        return null;
    }
}
