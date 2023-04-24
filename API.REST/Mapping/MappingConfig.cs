using DomainLayer.Objects;
using API.REST.DTO;

#if ProducesConsumes
#endif

namespace EmpAPI1.REST.Mapping
{
    public class MappingConfig : Infrastructure.Mapping.MappingConfig
    {
        public MappingConfig() : base()
        {
            CreateMap<UserDTO, Employee>().ReverseMap();
        }
    }
}
