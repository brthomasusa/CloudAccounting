using CloudAccounting.SharedKernel.Utilities;

namespace CloudAccounting.SharedKernel.Interfaces;

public interface IBusinessRule<T>
{
    void SetNext(IBusinessRule<T> next);

    Task<Result> Validate(T request);
}
