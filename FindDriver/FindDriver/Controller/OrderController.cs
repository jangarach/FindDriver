using FindDriver.Api.Model.Services;
using FindDriver.Api.View;
using Microsoft.AspNetCore.Mvc;

namespace FindDriver.Api.Controller
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

    }
}
