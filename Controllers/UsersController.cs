using LittleBank.Api.Database;
using LittleBank.Api.DTO;
using LittleBank.Api.Models;
using LittleBank.Api.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace LittleBank.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Получить пользователя")]
        [SwaggerResponse(200, "Пользователь")]
        [SwaggerResponse(404, "Пользователь не найден")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _context.Users.Select(x=> new
            {
                x.Id,
                x.Login,
                x.Role
            })
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return NotFound();

            return Ok(user);
        }


        [HttpPut]
        [SwaggerOperation(Summary = "Изменить данные пользователя")]
        [SwaggerResponse(404, "Пользователь не найден")]
        [SwaggerResponse(204, "Данные обновлены")]
        public async Task<IActionResult> Put([FromBody] UserDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == dto.Id);

            if (user is null)
                return NotFound();

            user.Login = dto.Login;
            user.Password = dto.Password;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Удалить пользователя")]
        [SwaggerResponse(404, "Клиента с таким id не существует")]
        [SwaggerResponse(400, "Клиент имеет невыплаченный кредит")]
        [SwaggerResponse(204, "Пользователь удален")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
                return NotFound();

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("types")]
        [SwaggerOperation(Summary = "Получить доступные роли пользователей")]
        [SwaggerResponse(200, "Доступные роли пользователи")]
        public IActionResult GetRoles()
        {
            var counter = 1;
            var list = Enum.GetNames(typeof(Roles));
            return Ok(list.Select(x => new
            {
                Id = counter++,
                Name = x
            }));
        }
    }
}