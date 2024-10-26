using Mapster;
using Models;
using HR.DTO.Outbound;

namespace HR.DTO.Mapper
{
    public class MapConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Operator, LoginResponse>.NewConfig();

        }
    }
}
