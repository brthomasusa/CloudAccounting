using CompanyDataModel = CloudAccounting.Infrastructure.Data.Models.Company;
using CompanyDomainModel = CloudAccounting.Core.Models.Company;

namespace CloudAccounting.Infrastructure.Data.Mappings;

public class CompanyMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Map Company domain model to Company data model
        config.NewConfig<CompanyDomainModel, CompanyDataModel>()
            .Map(dest => dest.CompanyCode, src => src.CompanyCode)
            .Map(dest => dest.CompanyName, src => src.CompanyName)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.Zipcode, src => src.Zipcode)
            .Map(dest => dest.Phone, src => src.Phone)
            .Map(dest => dest.Fax, src => src.Fax)
            .Map(dest => dest.Currency, src => src.Currency);

        // Map Company data model to Company domain model
        config.NewConfig<CompanyDataModel, CompanyDomainModel>()
            .Map(dest => dest.CompanyCode, src => src.CompanyCode)
            .Map(dest => dest.CompanyName, src => src.CompanyName)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.Zipcode, src => src.Zipcode)
            .Map(dest => dest.Phone, src => src.Phone)
            .Map(dest => dest.Fax, src => src.Fax)
            .Map(dest => dest.Currency, src => src.Currency);

    }
}
