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
    [Route("api/clients/{id:int}/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{cardId:int}")]
        [SwaggerOperation(Summary = "Получить карту клиента")]
        [SwaggerResponse(423, "Карта заблокирована")]
        [SwaggerResponse(404, "Карты не существует")]
        [SwaggerResponse(200, "Информация о карте")]
        public async Task<IActionResult> GetById(int id, int cardId)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(x => x.Client.Id == id && x.Id == cardId);

            if (card == null)
                return NotFound();

            if (!card.IsActive)
                return new StatusCodeResult(StatusCodes.Status423Locked);

            return Ok(card);
        }


        [HttpGet]
        [SwaggerOperation(
            Summary = "Получить карты пользователя")]
        [SwaggerResponse(404, "Клиент не найден")]
        [SwaggerResponse(200, "Клиент с информацией о незаблокированных картах")]
        public async Task<IActionResult> Get(int id)
        {
            var client = await _context.Clients.Select(c => new
            {
                c.Id,
                c.Name,
                Cards = c.Cards.Where(c => c.IsActive != false).Select(c => new
                {
                    c.Id,
                    c.Number,
                    c.Type
                })
            }).FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
                return NotFound();

            return Ok(client);
        }


        [HttpPost]
        [SwaggerOperation(
            Summary = "Создать новую карту клиента")]
        [SwaggerResponse(201, "Создано")]
        [SwaggerResponse(404, "Такого клиента не существует")]
        public async Task<IActionResult> Post([FromBody] CreateCardDTO dto, int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);

            if (client is null)
                return NotFound();

            var card = new Card()
            {
                Number = dto.Number,
                Type = dto.Type,
                Client = client
            };
            try
            {
                _context.Cards.Add(card);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction(nameof(GetById), new { cardId = card.Id }, card);
        }


        [HttpPatch]
        [SwaggerOperation(Summary = "Произвести денежную операцию с картой")]
        [SwaggerResponse(204, "Операция успешно проведена")]
        [SwaggerResponse(404, "Карта не найдена")]
        [SwaggerResponse(423, "Карта заблокирована")]
        [SwaggerResponse(422, "Операция не может быть проведена")]
        public async Task<IActionResult> Patch([FromBody] OperationCardDTO dto, int id)
        {
            var user = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);

            if (user is null)
                return NotFound("Карта не найдена");

            var card = await _context.Cards.FirstOrDefaultAsync(x => x.Id == dto.Id && x.Client.Id == id);

            if (card is null)
                return NotFound("Карта не найдена");

            if (card.IsActive)
                return new StatusCodeResult(StatusCodes.Status423Locked);

            if (dto.Operation == OperationTypes.Withdraw)
            {
                card.Sum -= dto.Sum;
                if (card.Sum < 0)
                    return new StatusCodeResult(StatusCodes.Status422UnprocessableEntity);

                card.Operations.Add(new Operation()
                {
                    Type = OperationTypes.Withdraw,
                    Value = dto.Sum
                });
            }
            else
            {
                card.Sum += dto.Sum;
                card.Operations.Add(new Operation()
                {
                    Type = OperationTypes.Accrual,
                    Value = dto.Sum
                });
            }

            _context.Cards.Update(card);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpHead("{cardId:int}")]
        [SwaggerOperation(Summary = "Заблокировать карту")]
        [SwaggerResponse(204, "Карта успешно заблокирована")]
        [SwaggerResponse(404, "Карта не найдена")]
        public async Task<IActionResult> Block(int cardId, int id)
        {
            var user = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);

            if (user is null)
                return NotFound("Карта не найдена");

            var card = await _context.Cards.FirstOrDefaultAsync(x=> x.Id == cardId && x.Client.Id == id);

            if (card is null)
                return NotFound("Карта не найдена");

            if (card.IsActive)
                return NoContent();

            card.IsActive = false;

            _context.Cards.Update(card);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("types")]
        [SwaggerOperation(Summary = "Получить все доступные типы карт")]
        [SwaggerResponse(200, "Типы карт")]
        public IActionResult GetCardTypes()
        {
            var counter = 1;
            var list = Enum.GetNames(typeof(CardTypes));
            return Ok(list.Select(x => new
            {
                Id = counter++,
                Name = x
            }));
        }
    }
}