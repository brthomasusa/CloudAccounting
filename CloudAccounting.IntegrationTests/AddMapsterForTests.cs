
using CloudAccounting.Application;

namespace CloudAccounting.IntegrationTests
{
    public static class AddMapsterForTests
    {
        public static Mapper GetMapper()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(InfrastructureAssembly.Instance, ApplicationAssembly.Instance);
            config.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);

            return new Mapper(config);
        }
    }
}