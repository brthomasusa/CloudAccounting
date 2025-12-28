using CloudAccounting.SharedKernel.Utilities;

namespace CloudAccounting.SharedKernel.Interfaces;

public interface ICommandHandler<TCommand>
{
    Task<Result<bool>> Handle(TCommand command);

    void SetNext(ICommandHandler<TCommand> next);
}
