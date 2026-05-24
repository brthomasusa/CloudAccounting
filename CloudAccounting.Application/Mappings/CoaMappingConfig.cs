using CloudAccounting.Application.UseCases.Coa.Create;
using CloudAccounting.Shared.Coa;

namespace CloudAccounting.Application.Mappings;

public class CoaMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Map CreateChartOfAccountCommand to ChartOfAccount
        config.NewConfig<CreateChartOfAccountCommand, ChartOfAccounts>()
            .Map(dest => dest.CompanyCode, src => src.CompanyCode)
            .Map(dest => dest.AccountCode, src => $"{src.LevelOne}{src.LevelTwo}{src.LevelThree}{src.LevelFour}")
            .Map(dest => dest.AccountTitle, src => src.AccountTitle)
            .Map(dest => dest.AccountLevel, src => GetCoaLevel(src.LevelOne))
            .Map(dest => dest.AccountClassification, src => GetClassification(src.LevelOne))
            .Map(dest => dest.AccountType, src => src.AccountType)
            .Map(dest => dest.CostCenterCode, src => src.CostCenterCode);
    }

    private static int GetCoaLevel(string levelOne)
    {
        return levelOne switch
        {
            "1" => 1,
            "2" => 2,
            "3" => 3,
            "4" => 4,
            "5" => 5,
            _ => throw new ArgumentException("Invalid Level One code. Must be 1-5.")
        };
    }

    private static string GetClassification(string levelOne)
    {
        return levelOne switch
        {
            "1" => "Asset",
            "2" => "Liability",
            "3" => "Equity",
            "4" => "Revenue",
            "5" => "Expense",
            _ => throw new ArgumentException("Invalid Level One code. Must be 1-5.")
        };
    }
}