using Mapster;
using Models;
using OP.DTO.Outbound;

namespace OP.DTO.Mapper
{
    public class MapConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Operator, LoginResponse>.NewConfig();

        }
    }
}
