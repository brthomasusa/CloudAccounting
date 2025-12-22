using CloudAccounting.DataContext;
using CloudAccounting.EntityModels.Entities;

namespace CloudAccounting.UnitTestWebApi;

public class EntityModelTests : TestBase
{
    [Fact]
    public void DatabaseConnectTest()
    {
        using CloudAccountingContext db = new();
        Assert.True(db.Database.CanConnect());
    }

    [Fact]
    public void CompanyCountTest()
    {
        using CloudAccountingContext db = new();

        int expected = 2;
        int actual = db.Companies.Count();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CompanyWithID1IsBtechnicalConsultingTest()
    {
        using CloudAccountingContext db = new();

        string expected = "BTechnical Consulting";

        Company? company = db.Companies.Find(keyValues: 1);
        string actual = company?.CompanyName ?? string.Empty;

        Assert.Equal(expected, actual);
    }
}
