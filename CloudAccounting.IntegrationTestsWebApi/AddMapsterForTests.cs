using CloudAccounting.Application;
using Mapster;
using MapsterMapper;

namespace CloudAccounting.IntegrationTestsWebApi
{
    public static class AddMapsterForTests
    {
        public static Mapper GetMapper()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(ApplicationAssembly.Instance);
            config.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);

            return new Mapper(config);
        }
    }
}