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
        }
    }
}
