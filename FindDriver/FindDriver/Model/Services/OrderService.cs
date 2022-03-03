using AutoMapper;
using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.DAL.Repositories;
using FindDriver.Api.Model.DAL.UI;
using System.Linq.Expressions;

namespace FindDriver.Api.Model.Services
{
    public interface IOrderService
    {
        Task<IList<OrderViewModel>> GetAllOrdersAsync();
        OrderViewModel FindOrder(int id);
        Task<OrderViewModel> FindOrderAsync(int id);
        Task<IList<OrderViewModel>> FindOrdersAsync(OrderFilter orderFilter);
        Task<OrderViewModel> CreateOrderAsync(OrderViewModel newOrder);
        Task<OrderViewModel> UpdateOrderAsync(int updateOrderId, OrderViewModel order);
        Task DeleteOrderAsync(OrderViewModel order);
    }
    public class OrderService : IOrderService
    {
        IMapper _mapper;
        IUserService _userService;
        IOrderRepository _orderRepository;
        IOrderReferencesService _orderReferencesService;

        public OrderService(IMapper mapper, 
                            IUserService userService,   
                            IOrderRepository orderRepository,
                            IOrderReferencesService orderReferencesService
            )
        {
            _mapper = mapper;
            _userService = userService;
            _orderRepository = orderRepository;
            _orderReferencesService = orderReferencesService;
        }
        public async Task<IList<OrderViewModel>> GetAllOrdersAsync()
        {
            var allOrders = await _orderRepository.GetAllAsync();
            if (allOrders == null || allOrders.Count == 0)
                throw new ApplicationException("Не удалось получить список всех заказов, возможно зиписи в БД отсутствуют");

            return await MapToViewModel(allOrders); ;
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

        public async Task<IList<OrderViewModel>> FindOrdersAsync(OrderFilter orderFilter)
        {
            var findedOrders = await _orderRepository.FindAllAsync(e =>
                e.Dateout >= orderFilter.DateOut.Date
                && (orderFilter.FromCityId != null ? e.FromCityId == orderFilter.FromCityId : true)
                && (orderFilter.ToCityId != null ? e.ToCityId == orderFilter.ToCityId : true)
                && (orderFilter.OrderTypeId != null ? e.OrderTypeId == orderFilter.OrderTypeId : true)
            );

            if (findedOrders == null || findedOrders.Count == 0)
                throw new ApplicationException("Не удалось найти список заказов!");

            return await MapToViewModel(findedOrders);
        }

        private async Task<IList<OrderViewModel>> MapToViewModel(IList<Order> orders)
        {
            var ordersViewModels = _mapper.Map<List<OrderViewModel>>(orders);

            var cityIds = ordersViewModels.Select(e => e.FromCityId)
                                         .Concat(ordersViewModels.Select(e => e.ToCityId))
                                         .Distinct().ToList();

            var userIds = ordersViewModels.Select(e => e.UserId).Distinct().ToList();

            //Tasks
            var citiesTask = _orderReferencesService.FindAllCitiesAsync(cityIds);
            var orderTypesTask = _orderReferencesService.GetAllOrderTypes();
            var usersTask = _userService.FindAllUsersAsync(e => userIds.Contains(e.Id));

            await Task.WhenAll(citiesTask, orderTypesTask, usersTask);

            //Tasks result
            var cities = await citiesTask;
            var orderTypes = await orderTypesTask;
            var users = await usersTask;

            SetNames(ordersViewModels, cities, orderTypes, users);

            return ordersViewModels;
        }

        private void SetNames(List<OrderViewModel> orderViewModels, IList<City> Cities, IList<OrderType> OrderTypes, IList<User> Users)
        {
            orderViewModels.ForEach(e =>
            {
                var orderType = OrderTypes.FirstOrDefault(c => c.Id == e.OrderTypeId);
                if (orderType != null)
                    e.OrderTypeName = orderType.TypeName;

                var fromCity = Cities.FirstOrDefault(c => c.Id == e.FromCityId);
                if (fromCity != null)
                    e.FromCityName = fromCity.Name;

                var toCity = Cities.FirstOrDefault(c => c.Id == e.ToCityId);
                if (toCity != null)
                    e.ToCityName = toCity.Name;

                var user = Users.FirstOrDefault(c => c.Id == e.UserId);
                if (user != null)
                {
                    e.UserName = user.Username;
                    e.UserPhoneNumber = user.PhoneNumber;
                }
            });
        }
    }
}
