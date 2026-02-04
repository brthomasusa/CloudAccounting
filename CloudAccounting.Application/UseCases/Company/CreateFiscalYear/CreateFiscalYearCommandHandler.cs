using CloudAccounting.Shared.Company;

namespace CloudAccounting.Application.UseCases.Company.CreateFiscalYear
{
    public class CreateFiscalYearCommandHandler
    (
    IFiscalYearRepository fiscalYearRepository,
    ICompanyService companyService,
    ILogger<CreateFiscalYearCommandHandler> logger,
    IMapper mapper
    ) : ICommandHandler<CreateFiscalYearCommand, CompanyWithFiscalPeriodsDto>
    {
        private readonly IFiscalYearRepository _fiscalRepo = fiscalYearRepository;
        private readonly ICompanyService _companyService = companyService;
        private readonly ILogger<CreateFiscalYearCommandHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<CompanyWithFiscalPeriodsDto>> Handle(CreateFiscalYearCommand command, CancellationToken token)
        {
            Result<FiscalYear> fiscalYearResult =
                await _companyService.CreateFiscalYearWithPeriods(command.CompanyCode, command.FiscalYear, command.StartMonthNumber);

            if (fiscalYearResult.IsFailure)
            {
                return Result<CompanyWithFiscalPeriodsDto>.Failure<CompanyWithFiscalPeriodsDto>(
                    new Error("CreateFiscalYearCommandHandler", fiscalYearResult.Error.Message)
                );
            }

            Result<FiscalYear> result = await _fiscalRepo.InsertFiscalYearAsync(fiscalYearResult.Value);

            if (result.IsFailure)
            {
                return Result<CompanyWithFiscalPeriodsDto>.Failure<CompanyWithFiscalPeriodsDto>(
                    new Error("CreateFiscalYearCommandHandler", result.Error.Message)
                );
            }

            CompanyWithFiscalPeriodsDto companyDto = _mapper.Map<CompanyWithFiscalPeriodsDto>(fiscalYearResult.Value);

            return companyDto;
        }
    }
}