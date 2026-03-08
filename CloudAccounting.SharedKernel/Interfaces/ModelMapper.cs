using CloudAccounting.SharedKernel.Utilities;

namespace CloudAccounting.SharedKernel.Interfaces;

public abstract class ModelMapper<TSource, TDestination>
{
    public abstract Result<TDestination> Map(TSource dataModel);
}
