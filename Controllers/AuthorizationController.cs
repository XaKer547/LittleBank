using LittleBank.Api.Database;
using LittleBank.Api.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace LittleBank.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AuthorizationController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        [SwaggerOperation(Summary = "Авторизация пользователя")]
        [SwaggerResponse(200,"Информация о пользователе с ролью")]
        [SwaggerResponse(404,"Ошибка авторизации")]
        public async Task<IActionResult> AuthorizeUser([FromBody] AuthorizationDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Password == dto.Password && u.Login == dto.Login);

            if (user is null)
            {
                return NotFound();
            }

            var claims = new Claim[]
            {
                new Claim("Role", user.Role.GetDisplayName()),
                new Claim("Id", user.Id.ToString())
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            return Ok(principal);
        }
    }
}
