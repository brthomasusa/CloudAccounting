using CloudAccounting.Shared.FiscalYear;

namespace CloudAccounting.Application.UseCases.FiscalYears.CreateFiscalYear
{
    public class CreateFiscalYearCommandHandler
    (
        IFiscalYearService fiscalYearService,
        ILogger<CreateFiscalYearCommandHandler> logger,
        IMapper mapper
    ) : ICommandHandler<CreateFiscalYearCommand, FiscalYearDto>
    {
        private readonly IFiscalYearService _fiscalYearService = fiscalYearService;
        private readonly ILogger<CreateFiscalYearCommandHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<FiscalYearDto>> Handle(CreateFiscalYearCommand command, CancellationToken token)
        {
            Result<FiscalYear> result =
                await _fiscalYearService.CreateFiscalYear(command.CompanyCode, command.FiscalYearNumber, command.StartDate);

            if (result.IsFailure)
            {
                return Result<FiscalYearDto>.Failure<FiscalYearDto>(
                    new Error("CreateFiscalYearCommandHandler.Handle", result.Error.Message)
                );
            }

            FiscalYearDto fiscalYearDto = result.Value.Adapt<FiscalYearDto>();

            return fiscalYearDto;
        }
    }
}