using FindDriver.Api.Model.DAL.UI;
using FindDriver.Api.Model.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FindDriver.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ControllerBase
    {
        IOrderService OrderService;
        public OrderController(IOrderService orderService)
        {
            OrderService = orderService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IList<OrderViewModel>?> GetAllOrders()
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
        [AllowAnonymous]
        public async Task<IList<OrderViewModel>?> GetFindedOrders([FromBody] OrderFilter filterOrder)
        {
            return await OrderService.FindOrdersAsync(filterOrder);
        }

        [HttpPost]
        public async Task<OrderViewModel?> UpdateOrder([FromBody]OrderViewModel order)
        {
            return await OrderService.UpdateOrderAsync(order.Id, order);
        }

    }
}
