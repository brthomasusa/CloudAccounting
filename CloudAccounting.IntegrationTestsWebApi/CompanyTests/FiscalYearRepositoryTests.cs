using FiscalYearDomainModel = CloudAccounting.Core.Models.FiscalYear;

namespace CloudAccounting.IntegrationTestsWebApi.CompanyTests
{
    public class FiscalYearRepositoryTests : TestBase
    {
        private readonly IFiscalYearRepository _fiscalYearRepo;
        private readonly IMapper _mapper = AddMapsterForTests.GetMapper();

        public FiscalYearRepositoryTests()
        {
            _fiscalYearRepo = new FiscalYearRepository(_efCoreContext!, _memoryCache!, new NullLogger<FiscalYearRepository>(), _dapperContext!, _mapper);
        }

        [Fact]
        public async Task InsertFiscalYearAsync_FiscalYearRepository_ShouldInsert12FiscalYearRecords()
        {
            // Arrange
            FiscalYearDomainModel fiscalYear = GetFiscalYearDomainModel();

            // Act
            Result<FiscalYearDomainModel> result = await _fiscalYearRepo.InsertFiscalYearAsync(fiscalYear);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(12, result.Value.FiscalPeriods.Count);
        }

        [Fact]
        public async Task GetFiscalYearByCompanyAndYearAsync_FiscalYearRepository_ShouldRetrieve12FiscalYearRecords()
        {
            // Arrange
            int companyCode = 4;
            int fiscalYearNumber = 2024;

            FiscalYearDomainModel fiscalYear = GetFiscalYearDomainModel();

            // Act
            Result<FiscalYearDomainModel> result = await _fiscalYearRepo.GetFiscalYearByCompanyAndYearAsync(companyCode, fiscalYearNumber);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(12, result.Value.FiscalPeriods.Count);
        }


        private static FiscalYearDomainModel GetFiscalYearDomainModel()
        {
            List<FiscalPeriod> fiscalPeriods = new()
            {
                new FiscalPeriod(1, "January", new DateTime(2025,1,1), new DateTime(2025,1,31), false),
                new FiscalPeriod(2, "February", new DateTime(2025,2,1), new DateTime(2025,2,28), false),
                new FiscalPeriod(3, "March", new DateTime(2025,3,1), new DateTime(2025,3,31), false),
                new FiscalPeriod(4, "April", new DateTime(2025,4,1), new DateTime(2025,4,30), false),
                new FiscalPeriod(5, "May", new DateTime(2025,5,1), new DateTime(2025,5,31), false),
                new FiscalPeriod(6, "June", new DateTime(2025,6,1), new DateTime(2025,6,30), false),
                new FiscalPeriod(7, "July", new DateTime(2025,7,1), new DateTime(2025,7,31), false),
                new FiscalPeriod(8, "August", new DateTime(2025,8,1), new DateTime(2025,8,31), false),
                new FiscalPeriod(9, "September", new DateTime(2025,9,1), new DateTime(2025,9,30), false),
                new FiscalPeriod(10, "October", new DateTime(2025,10,1), new DateTime(2025,10,31), false),
                new FiscalPeriod(11, "November", new DateTime(2025,11,1), new DateTime(2025,11,30), false),
                new FiscalPeriod(12, "December", new DateTime(2025,12,1), new DateTime(2025,12,31), false)
            };

            FiscalYearDomainModel fiscalYear =
                new
                (
                    2,
                    2025,
                    new DateTime(2025, 1, 1),
                    new DateTime(2025, 12, 31),
                    true,
                    false,
                    false,
                    DateTime.MinValue,
                    fiscalPeriods
                );

            return fiscalYear;
        }
    }
}