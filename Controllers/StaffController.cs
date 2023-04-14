using LittleBank.Api.Database;
using LittleBank.Api.DTO;
using LittleBank.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace LittleBank.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StaffController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [SwaggerOperation(Summary = "Получить сотрудников")]
        [SwaggerResponse(200, "Сотрудники")]
        public async Task<IActionResult> Get()
        {
            var staff = await _context.Employees.Select(x => new
            {
                x.Id,
                x.Name,
                x.Surname,
                x.Patronimyc,
            }).ToArrayAsync();

            return Ok(staff);
        }

        
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Получить информацию о сотруднике")]
        [SwaggerResponse(200, "Сотрудник")]
        public async Task<IActionResult> Get(int id)
        {
            var staff = await _context.Employees.Select(x => new
            {
                x.Id,
                x.Name,
                x.Surname,
                x.Patronimyc,
                x.Address,
                x.PhoneNumber
            }).FirstOrDefaultAsync(x => x.Id == id);

            return Ok(staff);
        }


        [HttpPost]
        [SwaggerOperation(Summary = "Добавить нового сотрудника")]
        [SwaggerResponse(202, "Сотрудника")]
        public async Task<IActionResult> Create(StaffCreateDTO model)
        {
            var employee = new Employee
            {
                Address = model.Address,
                PassportNumber = model.PassportNumber,
                PassportSeries = model.PassportSeries,
                Patronimyc = model.Patronimyc,
                PhoneNumber = model.PhoneNumber,
                Name = model.Name,
                Surname = model.Surname,
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }


        [HttpPost]
        [Route("{id:int}/users")]
        [SwaggerOperation(Summary = "Создать для сотрудника пользователя")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404, "Сотрудник не найден")]
        public async Task<IActionResult> CreateUserForStaff([FromRoute] int id, [FromBody] UserCreateDTO model)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee is null)
                return NotFound();

            var user = new User
            {
                Login = model.Login,
                Password = model.Password,
                Role = Models.Enums.Roles.Employee
            };

            employee.User = user;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
