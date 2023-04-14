using LittleBank.Api.Database;
using LittleBank.Api.DTO;
using LittleBank.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace LittleBank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [SwaggerOperation(Summary = "Получить всех клиентов")]
        [SwaggerResponse(200, "Клиенты")]
        public async Task<IActionResult> Get()
        {
            var clients = await _context.Clients.Select(x => new
            {
                x.Id,
                x.Name,
                x.Patronimyc,
                x.Surname,
                HaveCredit = x.Credits.Any()
            }).ToArrayAsync();

            return Ok(clients);
        }


        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Получает по клиента с информацией о всех картах")]
        [SwaggerResponse(200, "Клиент")]
        public async Task<IActionResult> Get(int id)
        {
            var client = await _context.Clients.Select(x => new
            {
                x.Id,
                x.Name,
                x.Patronimyc,
                x.Surname,
                x.PassportSeries,
                x.PassportNumber,
                Credits = x.Credits.Select(c => new
                {
                    c.CreateDate,
                    c.EndDate,
                    c.Sum
                })
            }).FirstOrDefaultAsync(x => x.Id == id);

            return Ok(client);
        }


        [HttpPost]
        [SwaggerOperation(Summary = "Создать нового клиента")]
        [SwaggerResponse(201, "Новый клиент")]
        public async Task<IActionResult> Create(ClientCreateDTO model)
        {
            var client = new Client
            {
                Name = model.Name,
                PassportNumber = model.PassportNumber,
                PassportSeries = model.PassportSeries,
                Patronimyc = model.Patronimyc,
                PhoneNumber = model.PhoneNumber,
                Surname = model.Surname
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = client.Id }, client);
        }


        [HttpPost]
        [Route("{id:int}/users")]
        [SwaggerOperation(Summary = "Создать новую учетную запись клиенту")]
        [SwaggerResponse(404, "Клиента не найден")]
        [SwaggerResponse(204)]
        public async Task<IActionResult> CreateUser([FromRoute] int id, [FromBody] UserCreateDTO model)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == id);

            if (client is null)
                return NotFound();

            var user = new User
            {
                Login = model.Login,
                Password = model.Password,
                Role = Models.Enums.Roles.User
            };

            client.User = user;

            _context.Clients.Update(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost]
        [Route("{id:int}/credits")]
        [SwaggerOperation(Summary = "Оформить кредит")]
        [SwaggerResponse(404, "Клиент не найден")]
        [SwaggerResponse(204, "Кредит оформлен")]
        public async Task<IActionResult> CreateCredit([FromRoute] int id, [FromBody] CreditCreateDTO model)
        {
            var client = await _context.Clients.SingleOrDefaultAsync(x => x.Id == id);

            if(client is null) 
                return NotFound();

            var credit = new Credit
            {
                CreateDate = model.CreateDate,
                EndDate = model.EndDate,
                Sum = model.Sum,
                Client = client
            };

            _context.Credits.Add(credit);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
