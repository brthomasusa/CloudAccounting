using CloudAccounting.Application.UseCases.Coa.Create;

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
            .Map(dest => dest.AccountLevel,
                src => GetCoaLevel($"{src.LevelOne}{src.LevelTwo}{src.LevelThree}{src.LevelFour}"))
            .Map(dest => dest.AccountClassification, src => GetClassification(src.LevelOne))
            .Map(dest => dest.AccountType, src => src.AccountType)
            .Map(dest => dest.CostCenterCode, src => src.CostCenterCode);
    }

    private static int GetCoaLevel(string acctCode)
    {
        return acctCode.Length switch
        {
            1 => 1,
            3 => 2,
            6 => 3,
            11 => 4,
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