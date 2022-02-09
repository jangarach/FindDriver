using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.Services;
using Microsoft.AspNetCore.Mvc;

namespace FindDriver.Api.Controller
{
    [Route("api/[controller]/[action]")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }
        [HttpGet]
        public async Task<IList<City>> GetAllCities()
        {
            return await _cityService.GetAllCities();
        }
    }
}
