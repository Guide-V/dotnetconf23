using Microsoft.AspNetCore.Mvc;
using VendingMachineService.Models;

namespace VendingMachineService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrinkController : Controller
    {
        private readonly ILogger<DrinkController> _logger;
        public DrinkController(ILogger<DrinkController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "OrderDrink")]
        public DrinkOrder Post(DrinkOrder drinkOrder)
        {
            return new DrinkOrder
            {
                Id = drinkOrder.Id,
                Price = drinkOrder.Price,
                CreatedDate = DateTime.Now,
                HasStraw = drinkOrder.HasStraw
            };
        }
    }
}
