using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.Services;
using Microsoft.AspNetCore.Mvc;

namespace FindDriver.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OrderReferencesController : ControllerBase
    {
        private readonly IOrderReferencesService _orderReferencesService;
        public OrderReferencesController(IOrderReferencesService orderReferencesService)
        {
            _orderReferencesService = orderReferencesService;
        }
        [HttpGet]
        public async Task<IList<City>> GetAllCities()
        {
            return await _orderReferencesService.GetAllCities();
        }

        [HttpGet]
        public async Task<IList<OrderType>> GetAllOrderTypes()
        {
            return await _orderReferencesService.GetAllOrderTypes();
        }
    }
}
