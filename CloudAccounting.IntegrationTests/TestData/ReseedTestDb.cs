namespace CloudAccounting.IntegrationTests.TestData;

public static class ReseedTestDb
{
    public static async Task ReseedTestDbAsync(AppDbContext context)
        => await context.Database.ExecuteSqlRawAsync("dbo.usp_ReseedCloudAcctgTestDb;");
}
