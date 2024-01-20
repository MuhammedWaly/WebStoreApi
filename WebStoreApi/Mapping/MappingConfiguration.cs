using AutoMapper;
using WebStoreApi.Models;
using WebStoreApi.Models.DTOS;

namespace WebStoreApi.Mapping
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<Contact, ContactDto>().ReverseMap();
            CreateMap<ApplicationUser, UserProfileDto>().ReverseMap();
            CreateMap<Order, OrderResponseDto>().ReverseMap();
            //CreateMap<List<Order>, List<OrderResponseDto>>().ReverseMap();
        }
    }
}
