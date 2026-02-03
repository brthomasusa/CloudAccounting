using CloudAccounting.Application.ViewModels.Company;
using CloudAccounting.Application.UseCases.CreateCompany;
using CloudAccounting.Core.Models;
using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.Mappings;

public class CompanyMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Map CompanyDetailVm to Company
        config.NewConfig<CompanyDetailVm, Company>()
            .Map(dest => dest.CompanyCode, src => src.CompanyCode)
            .Map(dest => dest.CompanyName, src => src.CompanyName)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.Zipcode, src => src.Zipcode)
            .Map(dest => dest.Phone, src => src.Phone)
            .Map(dest => dest.Fax, src => src.Fax)
            .Map(dest => dest.Currency, src => src.Currency);

        // Map Company to CompanyDetailVm
        config.NewConfig<Company, CompanyDetailVm>()
            .Map(dest => dest.CompanyCode, src => src.CompanyCode)
            .Map(dest => dest.CompanyName, src => src.CompanyName)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.Zipcode, src => src.Zipcode)
            .Map(dest => dest.Phone, src => src.Phone)
            .Map(dest => dest.Fax, src => src.Fax)
            .Map(dest => dest.Currency, src => src.Currency);

        // Map CreateCompanyCommand to Company
        config.NewConfig<CreateCompanyCommand, Company>()
            .Map(dest => dest.CompanyCode, src => src.CompanyCode)
            .Map(dest => dest.CompanyName, src => src.CompanyName)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.Zipcode, src => src.Zipcode)
            .Map(dest => dest.Phone, src => src.Phone)
            .Map(dest => dest.Fax, src => src.Fax)
            .Map(dest => dest.Currency, src => src.Currency);

        // Map FiscalYear domain obj to CompanyWithFiscalPeriodsDto
        config.NewConfig<FiscalYear, CompanyWithFiscalPeriodsDto>()
            .Map(dest => dest.CompanyCode, src => src.CompanyCode)
            .Map(dest => dest.CompanyName, src => src.CompanyName)
            .Map(dest => dest.FiscalYear, src => src.Year)
            .Map(dest => dest.IsInitialYear, src => src.IsInitialFiscalYear)
            .Map(dest => dest.FiscalPeriods, src => src.FiscalPeriods);

    }
}
