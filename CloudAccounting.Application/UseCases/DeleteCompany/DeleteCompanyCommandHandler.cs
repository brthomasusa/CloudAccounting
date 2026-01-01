using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudAccounting.Application.UseCases.DeleteCompany
{
    public class DeleteCompanyCommandHandler
    (
    ICompanyRepository repository,
    ILogger<DeleteCompanyCommandHandler> logger
    ) : ICommandHandler<DeleteCompanyCommand, MediatR.Unit>
    {
        private readonly ICompanyRepository _repository = repository;
        private readonly ILogger<DeleteCompanyCommandHandler> _logger = logger;

        public async Task<Result<MediatR.Unit>> Handle(DeleteCompanyCommand command, CancellationToken token)
        {
            Result result = await _repository.DeleteAsync(command.CompanyCode);

            if (result.IsFailure)
            {
                return Result<MediatR.Unit>.Failure<MediatR.Unit>(
                    new Error("DeleteCompanyCommandHandler.Handle", result.Error.Message)
                );
            }

            return MediatR.Unit.Value;
        }
    }
}