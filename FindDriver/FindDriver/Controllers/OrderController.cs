using FindDriver.Api.Model.DAL.UI;
using FindDriver.Api.Model.Services;
using Microsoft.AspNetCore.Mvc;

namespace FindDriver.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        IOrderService OrderService;
        public OrderController(IOrderService orderService)
        {
            OrderService = orderService;
        }

        [HttpGet]
        public async Task<IList<OrderViewModel>> GetAllOrders()
        {
            return await OrderService.GetAllOrdersAsync();
        }

        [HttpGet]
        public async Task<OrderViewModel> GetOrderById(int id)
        {
            return await OrderService.FindOrderAsync(id);
        }

        [HttpPost]
        public async Task<OrderViewModel> CreateOrder([FromBody]OrderViewModel order)
        {
            if (order == null)
                throw new ApplicationException("Добавляемый заказ пустой");
            return await OrderService.CreateOrderAsync(order);
        }

        [HttpPost]
        public async Task<IList<OrderViewModel>> GetFindedOrders([FromBody] OrderFilter filterOrder)
        {
            return await OrderService.FindOrdersAsync(filterOrder);
        }

    }
}
