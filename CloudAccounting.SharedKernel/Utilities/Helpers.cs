namespace CloudAccounting.SharedKernel.Utilities;

public static class Helpers
{
    public static string GetInnerExceptionMessage(Exception ex)
        => ex.InnerException == null ? ex.Message : ex.InnerException.Message;

}
