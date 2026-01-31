using CloudAccounting.Core.Models;

namespace CloudAccounting.UnitTests.TestData
{
    public static class CompanyTestData
    {
        public static Company GetCompanyForCreate()
            => new()
            {
                CompanyCode = 0,
                CompanyName = "New Company, Inc.",
                Address = "12345 Munger Ave",
                City = "Dallas",
                Zipcode = "75214",
                Phone = "469-555-1234",
                Fax = "469-555-2345",
                Currency = "USD"
            };
    }
}