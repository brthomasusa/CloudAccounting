namespace CloudAccounting.Application.UseCases.CreateCompany;

public class CreateCompanyCommandHandler
(
    ICompanyRepository repository,
    ILogger<CreateCompanyCommandHandler> logger,
    IMapper mapper
) : ICommandHandler<CreateCompanyCommand, int>
{
    private readonly ICompanyRepository _repository = repository;
    private readonly ILogger<CreateCompanyCommandHandler> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<int>> Handle(CreateCompanyCommand command, CancellationToken token)
    {
        Company company = _mapper.Map<Company>(command);
        Result<Company> createCompanyResult = await _repository.CreateAsync(company);

        if (createCompanyResult.IsFailure)
        {
            return Result<int>.Failure<int>(
                new Error("GetCompanyByIdQueryHandler.Handle", createCompanyResult.Error.Message)
            );
        }

        return createCompanyResult.Value.CompanyCode;
    }
}
