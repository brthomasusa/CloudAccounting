using System.Reflection;

namespace CloudAccounting.Web;

public static class ServerAssembly
{
    public static readonly Assembly Instance = typeof(ServerAssembly).Assembly;
}
