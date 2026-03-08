namespace CloudAccounting.IntegrationTests;

[CollectionDefinition("SequentialTestCollection", DisableParallelization = true)]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // Class used only for Collection Definition
}
