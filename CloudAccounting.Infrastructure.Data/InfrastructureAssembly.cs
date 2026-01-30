using System.Reflection;

namespace CloudAccounting.Infrastructure.Data;

public static class InfrastructureAssembly
{
    public static readonly Assembly Instance = typeof(InfrastructureAssembly).Assembly;
}
