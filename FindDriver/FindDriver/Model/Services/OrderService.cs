using AutoMapper;
using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.DAL.Repositories;
using FindDriver.Api.View;

namespace FindDriver.Api.Model.Services
{
    public interface IOrderService
    {
        Task<IList<OrderViewModel>> GetAllOrdersAsync();
        OrderViewModel FindOrder(int id);
        Task<OrderViewModel> FindOrderAsync(int id);
        Task<OrderViewModel> CreateOrderAsync(OrderViewModel newOrder);
        Task<OrderViewModel> UpdateOrderAsync(int updateOrderId, OrderViewModel order);
        Task DeleteOrderAsync(OrderViewModel order);
    }
    public class OrderService : IOrderService
    {
        IMapper _mapper;
        ICityService _cityService;
        IOrderRepository _orderRepository;
        
        public OrderService(IMapper mapper, 
                            IOrderRepository orderRepository,
                            ICityService cityService
            )
        {
            _mapper = mapper;
            _cityService = cityService;
            _orderRepository = orderRepository;
        }
        public async Task<IList<OrderViewModel>> GetAllOrdersAsync()
        {
            var allOrders = await _orderRepository.GetAllAsync();
            if (allOrders == null || allOrders.Count == 0)
                throw new ApplicationException("Не удалось получить список всех заказов, возможно зиписи в БД отсутствуют");
                
            var ordersViewModels = _mapper.Map<List<OrderViewModel>>(allOrders);

            var cityIds = ordersViewModels.Select(e => e.FromCityId)
                                         .Concat(ordersViewModels.Select(e => e.ToCityId))
                                         .Distinct().ToList();
                                         
            var cities = await _cityService.FindAllAsync(cityIds);

            SetCityNames(ordersViewModels, cities.ToList());

            return ordersViewModels;
        }
        public OrderViewModel FindOrder(int id)
        {
            throw new NotImplementedException();
        }
        public Task<OrderViewModel> FindOrderAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<OrderViewModel> CreateOrderAsync(OrderViewModel newOrder)
        {
            var order = _mapper.Map<Order>(newOrder);
            await _orderRepository.InsertAsync(order);

            return _mapper.Map<OrderViewModel>(order);
        }
        public Task DeleteOrderAsync(OrderViewModel order)
        {
            throw new NotImplementedException();
        }
        public Task<OrderViewModel> UpdateOrderAsync(int updateOrderId, OrderViewModel order)
        {
            throw new NotImplementedException();
        }

        private void SetCityNames(List<OrderViewModel> orderViewModels, List<City> Cities)
        {
            orderViewModels.ForEach(e =>
            {
                var fromCity = Cities.FirstOrDefault(c => c.Id == e.FromCityId);
                if (fromCity != null)
                    e.FromCityName = fromCity.Name;

                var toCity = Cities.FirstOrDefault(c => c.Id == e.ToCityId);
                if (toCity != null)
                    e.ToCityName = toCity.Name;
            });
        }
    }
}
