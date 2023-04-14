//using LittleBank.Api.Database;
//using LittleBank.Api.DTO;
//using LittleBank.Api.Models.Enums;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace LittleBank.Api.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class OperationsController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        public OperationsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetUserOperations([FromQuery] OperationsFilterDTO? dto, int clientId)
//        {
//            var operations = await _context.Clients.Select(c => new
//            {
//                c.Id,
//                Cards = c.Cards.Select(c => new
//                {
//                    c.Id,
//                    c.Operations
//                }),
//            }).FirstOrDefaultAsync(c => c.Id == clientId);

//            if (operations is null)
//                return NotFound("Такого пользователя не существует");

//            if (dto.Type.HasValue)
//                //Да блять как сука нахуй тебя сделать пидрила ебаная!?!?!?
//                operations.Cards.Select(c =>
//                {
//                    return Ok(new
//                    {
//                        c.Id,
//                        Operations = c.Operations
//                       .Any(o => o.Type == dto.Type)
//                    });
//                });

//            return Ok(operations);
//        }

//        [HttpGet("types")]
//        public IActionResult GetOperations()
//        {
//            var counter = 1;
//            var list = Enum.GetNames(typeof(OperationTypes));
//            return Ok(list.Select(x => new
//            {
//                Id = counter++,
//                Name = x
//            }));
//        }





//        //операции все или конкретного чела?
//        //в операциях есть только снятие и вложение или что-то еще?

//        ///еще что-то будет?
//    }
//}
