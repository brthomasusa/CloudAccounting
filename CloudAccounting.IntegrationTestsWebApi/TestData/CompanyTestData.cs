using CloudAccounting.Core.Models;

namespace CloudAccounting.IntegrationTestsWebApi.TestData
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
                Currency = "$"
            };

        public static Company GetCompanyForUpdate()
            => new()
            {
                CompanyCode = 2,
                CompanyName = "Updated Company, LLC.",
                Address = "99 Henderson Street",
                City = "Dallas",
                Zipcode = "75214",
                Phone = "469-555-1234",
                Fax = "469-555-2345",
                Currency = "$"
            };
    }
}