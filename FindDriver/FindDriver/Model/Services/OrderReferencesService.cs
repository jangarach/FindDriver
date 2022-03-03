using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.DAL.Repositories;

namespace FindDriver.Api.Model.Services
{
    public interface IOrderReferencesService
    {
        Task<IList<City>> GetAllCities();
        Task<IList<OrderType>> GetAllOrderTypes();
        Task<IList<City>> FindAllCitiesAsync(List<int> pkIds);
    }
    public class OrderReferencesService : IOrderReferencesService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IOrderTypeRepository _orderTypeRepository;
        public OrderReferencesService(ICityRepository cityRepository, IOrderTypeRepository orderTypeRepository)
        {
            _cityRepository = cityRepository;
            _orderTypeRepository = orderTypeRepository;
        }

        public async Task<IList<City>> GetAllCities()
        {
            return await _cityRepository.GetAllAsync();
        }

        public async Task<IList<City>> FindAllCitiesAsync(List<int> pkIds)
        {
            var res = await _cityRepository.FindAllAsync(e => pkIds.Contains(e.Id));
            return res.ToList();
        }

        public async Task<IList<OrderType>> GetAllOrderTypes()
        {
            return await _orderTypeRepository.GetAllAsync();
        }
    }
}
