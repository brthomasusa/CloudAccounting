
using CloudAccounting.Application.UseCases.Coa.Create;

namespace CloudAccounting.IntegrationTests.ChartOfAccountTest;

[Collection("SequentialTestCollection")]
public class CoaCommandValidatorTests(DatabaseFixture fixture) : IAsyncLifetime
{
    private readonly AppDbContext _context = fixture.Context!;
    private readonly IMemoryCache? _memoryCache = fixture.MemoryCache;

    private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

    private IChartOfAccountRepository CoaRepositoryepo =>
        new ChartOfAccountRepository(_context, new NullLogger<ChartOfAccountRepository>());

    private ICompanyRepository CompanyRepository =>
        new CompanyRepository(_context, _memoryCache!, new NullLogger<CompanyRepository>(), _mapper);

    private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase!();

    [Fact]
    public async Task CreateChartOfAccountCommandValidator_ShouldHaveNoValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateChartOfAccountCommandValidator validator = new(CoaRepositoryepo, CompanyRepository);
        var command = CreateValidCommand(); // 30100200002

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateChartOfAccountCommandValidator_InvalidCompanyCode_ShouldHaveValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateChartOfAccountCommandValidator validator = new(CoaRepositoryepo, CompanyRepository);
        var command = CreateValidCommand(); // 30100200002
        command.CompanyCode = 999; // Invalid company code

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.CompanyCode);
    }

    [Fact]
    public async Task CreateChartOfAccountCommandValidator_InvalidLevelOne_ShouldHaveValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateChartOfAccountCommandValidator validator = new(CoaRepositoryepo, CompanyRepository);
        var command = CreateValidCommand(); // 30100200002
        command.LevelOne = "9"; // Invalid level one

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.LevelOne);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("000")]
    [InlineData("XX")]
    public async Task CreateChartOfAccountCommandValidator_InvalidLevelTwo_ShouldHaveValidationErrors(
        string invalidLevelTwo)
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateChartOfAccountCommandValidator validator = new(CoaRepositoryepo, CompanyRepository);
        var command = CreateValidCommand();
        command.LevelTwo = invalidLevelTwo; // Invalid level two

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.LevelTwo);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("00")]
    [InlineData("0000")]
    [InlineData("XXK")]
    public async Task CreateChartOfAccountCommandValidator_InvalidLevelThree_ShouldHaveValidationErrors(
        string invalidLevelThree)
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateChartOfAccountCommandValidator validator = new(CoaRepositoryepo, CompanyRepository);
        var command = CreateValidCommand();
        command.LevelThree = invalidLevelThree; // Invalid level three

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.LevelThree);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("00")]
    [InlineData("000")]
    [InlineData("0000")]
    [InlineData("000000")]
    [InlineData("XXKA")]
    public async Task CreateChartOfAccountCommandValidator_InvalidLevelFour_ShouldHaveValidationErrors(
        string invalidLevelFour)
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateChartOfAccountCommandValidator validator = new(CoaRepositoryepo, CompanyRepository);
        var command = CreateValidCommand();
        command.LevelFour = invalidLevelFour; // Invalid level four

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.LevelFour);
    }

    [Fact]
    public async Task CreateChartOfAccountCommandValidator_CreateLevelTwo_ShouldHaveNoValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateChartOfAccountCommandValidator validator = new(CoaRepositoryepo, CompanyRepository);
        var command = CreateValidCommand();
        command.LevelTwo = "03";
        command.LevelThree = null!;
        command.LevelFour = null!;

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateChartOfAccountCommandValidator_CreateLevelThree_ShouldHaveNoValidationErrors()
    {
        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateChartOfAccountCommandValidator validator = new(CoaRepositoryepo, CompanyRepository);
        var command = CreateValidCommand();
        command.LevelThree = "004";
        command.LevelFour = null!;

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateChartOfAccountCommandValidator_MissingLevelTwoParent_ShouldHaveValidationErrors()
    {
        // Account code length (9 characters) is invalid and parent account does not exist for level three account

        // Arrange
        await ReseedTestDb.ReseedTestDbAsync(_context);
        CreateChartOfAccountCommandValidator validator = new(CoaRepositoryepo, CompanyRepository);
        var command = CreateValidCommand(); // 30100200002
        command.LevelTwo = null!;

        // Act
        var result = await validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrors();
    }


    private CreateChartOfAccountCommand CreateValidCommand()
    {
        return new CreateChartOfAccountCommand
        {
            CompanyCode = 1,
            LevelOne = "3",
            LevelTwo = "01",
            LevelThree = "002",
            LevelFour = "00003",
            AccountTitle = "Test Account",
            AccountType = "Other",
            CostCenterCode = "09001"
        };
    }
}