#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.

using MediatR;

namespace CloudAccounting.Application.Behaviors
{
    public sealed class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse?> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next(cancellationToken);
            }

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors)
                                            .Where(f => f != null)
                                            .Select(failure => new CloudAccounting.SharedKernel.Exceptions.ValidationError(
                                                failure.PropertyName,
                                                failure.ErrorMessage))
                                            .Distinct()
                                            .ToList();

            if (failures.Count != 0)
            {
                throw new CloudAccounting.SharedKernel.Exceptions.ValidationException(failures);
            }
            
            return await next(cancellationToken);
        }
    }
}