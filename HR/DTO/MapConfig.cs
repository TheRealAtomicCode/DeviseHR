using Mapster;
using Models;

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
