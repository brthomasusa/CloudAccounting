using MediatR;
using CloudAccounting.SharedKernel.Utilities;

namespace CloudAccounting.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;

