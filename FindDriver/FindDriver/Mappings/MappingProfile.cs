using AutoMapper;
using FindDriver.Api.Model.DAL.DTO;
using FindDriver.Api.Model.DAL.UI;

namespace FindDriver.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderViewModel>().ReverseMap();
        }
    }
}
