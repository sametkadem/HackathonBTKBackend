using AppBackend.DtoLayer.Dtos.IdentityDto.LoginDto;
using AppBackend.DtoLayer.Dtos.IdentityDto.RegisterDto;
using AppBackend.DtoLayer.Dtos.IdentityDto.UserRoleDto;
using AppBackend.EntityLayer.Concrete.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using randevuburada.EntityLayer.Concrete.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppBackend.WebApi.Controllers.Identity
{
    [Route("api/v1/account/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] CreateNewUserDto createNewUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Status = 0,
                    Message = "Girdi verilerinde hata var",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            try
            {
                var existingUser = await _userManager.FindByNameAsync(createNewUserDto.UserName) ??
                                   await _userManager.FindByEmailAsync(createNewUserDto.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { Status = 0, Message = "Kullanıcı adı veya email zaten mevcut" });
                }
                var user = new AppUser
                {
                    UserName = createNewUserDto.UserName,
                    Email = createNewUserDto.Email,
                    FirstName = createNewUserDto.FirstName,
                    LastName = createNewUserDto.LastName,
                    PhoneNumber = createNewUserDto.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, createNewUserDto.Password);
                if (result.Succeeded)
                {
                    return Ok(new { Status = 1, Message = "Kullanıcı başarıyla oluşturuldu" });
                }
                return BadRequest(new { Status = 0, Message = "Kullanıcı oluşturulurken hata oluştu", Errors = result.Errors });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 0, Message = "Sunucu hatası", Error = ex.Message });
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Status = 0,
                    Message = "Girdi verilerinde hata var",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                AppUser user = null;
                if (loginDto.Identifier.Contains("@"))
                {
                    user = await _userManager.FindByEmailAsync(loginDto.Identifier);
                }
                else
                {
                    user = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.UserName == loginDto.Identifier || u.PhoneNumber == loginDto.Identifier);
                }

                if (user == null)
                {
                    return BadRequest(new { Status = 0, Message = "Kullanıcı bulunamadı" });
                }

                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (result)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id.ToString())
            };

                    authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"]!)),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                            SecurityAlgorithms.HmacSha256
                        )
                    );

                    return Ok(new { Status = 1, Token = new JwtSecurityTokenHandler().WriteToken(token) });
                }

                return BadRequest(new { Status = 0, Message = "Kullanıcı adı veya şifre hatalı" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = 0, Message = "Bir hata oluştu", Error = ex.Message });
            }
        }


    }
}
