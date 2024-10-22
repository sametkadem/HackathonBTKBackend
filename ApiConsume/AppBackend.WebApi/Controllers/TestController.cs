using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppBackend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("Get")]
        public IActionResult Get()
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { Status = 0, Message = "Kullanıcı oturumu açık değil." });
            }

            // Retrieve the user ID from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return BadRequest(new { Status = 0, Message = "Kullanıcı ID bulunamadı." });
            }

            // Convert userId to int (or whatever type it is in your application)
            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                // Now you can use the userId for your logic
                return Ok(new { Status = 1, UserId = userId });
            }

            return BadRequest(new { Status = 0, Message = "Geçersiz kullanıcı ID." });
        }
    }
}
