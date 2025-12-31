using CloudAccounting.Application.UseCases.CreateCompany;
using CloudAccounting.Application.UseCases.UpdateCompany;
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
                Currency = "USD"
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
                Currency = "USD"
            };

        public static CreateCompanyCommand GetCreateCompanyCommand()
            => new()
            {
                CompanyCode = 0,
                CompanyName = "New Company, Inc.",
                Address = "12345 Mulholland Ave",
                City = "Dallas",
                Zipcode = "75214",
                Phone = "469-555-1234",
                Fax = "469-555-2345",
                Currency = "USD"
            };

        public static UpdateCompanyCommand GetUpdateCompanyCommand()
            => new()
            {
                CompanyCode = 1,
                CompanyName = "BTechnical Consulting",
                Address = "12345 Mulholland Ave",
                City = "Dallas",
                Zipcode = "75214",
                Phone = "469-555-1234",
                Fax = "469-555-2345",
                Currency = "USD"
            };
    }
}