namespace CloudAccounting.Application.UseCases.Coa.Create;

public class CreateChartOfAccountCommandValidator : AbstractValidator<CreateChartOfAccountCommand>
{
    private readonly IChartOfAccountRepository _repository;
    private readonly ICompanyRepository _companyRepository;

    public CreateChartOfAccountCommandValidator
    (
        IChartOfAccountRepository repository,
        ICompanyRepository companyRepository
    )
    {
        _repository = repository;
        _companyRepository = companyRepository;

        RuleFor(x => x.CompanyCode)
            .GreaterThan(0)
            .MustAsync(ValidateCompanyCode)
            .WithMessage("The company code is not valid.");

        RuleFor(x => x.LevelOne)
            .NotEmpty()
            .Must(val => new[] { "1", "2", "3", "4", "5" }.Contains(val))
            .WithMessage("'{LevelOne}' must be one of the following values: 1, 2, 3, 4, 5.");

        RuleFor(x => x.LevelTwo)
            .Matches("^\\d{2}$")
            .When(x => !string.IsNullOrEmpty(x.LevelTwo))
            .WithMessage("'{LevelTwo}' must be numeric and exactly 2 characters long.");

        RuleFor(x => x.LevelThree)
            .Matches("^\\d{3}$")
            .When(x => !string.IsNullOrEmpty(x.LevelThree))
            .WithMessage("'{LevelThree}' must be numeric and exactly 3 characters long.");

        RuleFor(x => x.LevelFour)
            .Matches("^\\d{5}$")
            .When(x => !string.IsNullOrEmpty(x.LevelFour))
            .WithMessage("'{LevelFour}' must be numeric and exactly 5 characters long.");

        RuleFor(x => x.AccountTitle).NotEmpty().MaximumLength(50);

        RuleFor(x => x).MustAsync(ValidateAccountCodeLength)
            .WithMessage(
                "The combined length of all the levels in the account code is not valid. Allowed lengths are 1, 3, 6, or 11 characters.");

        RuleFor(x => x).MustAsync(IsExistingAccount).WithMessage("An account with the same code already exists.");

        RuleFor(x => x).MustAsync(HasParentAccount)
            .WithMessage(
                "All level 2, 3, and 4 accounts must have an existing parent account. Please ensure the parent account exists before creating this account.");

        RuleFor(x => x).MustAsync(CheckForGapsInAccountHierarchy)
            .WithMessage(
                """
                There are gaps in the account hierarchy. 
                                Please ensure that if Level Three is provided, 
                                then Level Two must also be provided, 
                                and if Level Four is provided, then Level Three must also be provided.
                """);
    }

    private async Task<bool> IsExistingAccount(CreateChartOfAccountCommand command, CancellationToken cancellationToken)
    {
        var accountCode =
            $"{command.LevelOne}{command.LevelTwo ?? ""}{command.LevelThree ?? ""}{command.LevelFour ?? ""}";

        var result = await _repository.IsExistingAccount(command.CompanyCode, accountCode);

        return !result.Value; // Return true if account does NOT exist
    }

    private async Task<bool> ValidateCompanyCode(int companyCode, CancellationToken cancellationToken)
    {
        Result<bool> result = await _companyRepository.IsExistingCompany(companyCode);

        return result.Value;
    }

    private static Task<bool> ValidateAccountCodeLength(CreateChartOfAccountCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var accountCode =
                $"{command.LevelOne}{command.LevelTwo ?? ""}{command.LevelThree ?? ""}{command.LevelFour ?? ""}";

            var totalLength = accountCode.Length;

            var result = totalLength switch
            {
                1 => true,
                3 => true,
                6 => true,
                11 => true,
                _ => false // The discard pattern (default)
            };

            return Task.FromResult(result); // Example validation, adjust as needed
        }
        catch (Exception exception)
        {
            return Task.FromException<bool>(exception);
        }
    }

    private async Task<bool> HasParentAccount(CreateChartOfAccountCommand command, CancellationToken cancellationToken)
    {
        var accountCode =
            $"{command.LevelOne}{command.LevelTwo ?? ""}{command.LevelThree ?? ""}{command.LevelFour ?? ""}";

        var parentAccountCode = accountCode.Length switch
        {
            3 => command.LevelOne,
            6 => $"{command.LevelOne}{command.LevelTwo}",
            11 => $"{command.LevelOne}{command.LevelTwo}{command.LevelThree}",
            _ => null
        };

        var result = await _repository.IsExistingAccount(command.CompanyCode, parentAccountCode!);

        return result.Value; // Return true if parent account exists
    }

    private Task<bool> CheckForGapsInAccountHierarchy(CreateChartOfAccountCommand command, CancellationToken
        cancellationToken)
    {
        if (string.IsNullOrEmpty(command.LevelTwo))
        {
            return Task.FromResult(command.LevelThree == null &&
                                   command.LevelFour == null); // No parent account needed for level 1
        }

        return string.IsNullOrEmpty(command.LevelThree)
            ? Task.FromResult(command.LevelFour == null)
            : Task.FromResult(true);
    }
}