using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.DAL.Repositories;

namespace FindDriver.Api.Model.Services
{
    public interface ICityService
    {
        Task<IList<City>> GetAllCities();
        Task<IList<City>> FindAllAsync(List<int> keyIds);
    }
    public class CityService : ICityService
    {
        ICityRepository _cityRepository;
        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        public async Task<IList<City>> FindAllAsync(List<int> keyIds)
        {
            return await _cityRepository.FindAllAsync(keyIds);
        }
        public async Task<IList<City>> GetAllCities()
        {
            return await _cityRepository.GetAllAsync();
        }
    }
}
