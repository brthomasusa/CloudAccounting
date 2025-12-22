using CloudAccounting.EntityModels.Entities;
using Microsoft.Extensions.Caching.Hybrid;
using NSubstitute;
using Xunit;

namespace CloudAccounting.UnitTestWebApi
{
    public class CompanyRepositoryTests : TestBase
    {
        private readonly HybridCache _mockCache = Substitute.For<HybridCache>();



        [Fact]
        public void Test1()
        {
            // Arrange
            // Company company = new()
            // {
            //     CompanyCode = 1,
            //     CompanyName = "BTechnical Consulting"
            // };
            // string expectedValue = company.CompanyName;
            // string cacheKey = "1";

            // // Mock the GetOrCreateAsync method to return the expected value immediately
            // // NSubstitute can handle the complex arguments, including the Func callback
            // _mockCache.GetOrCreateAsync<Company>(
            //     cacheKey,
            //     Arg.Any<Func<CancellationToken, ValueTask<Company>>>(),
            //     Arg.Any<HybridCacheEntryOptions>(),
            //     company,
            //     Arg.Any<CancellationToken>()
            // ).Returns(new ValueTask<string>(expectedValue));


            Assert.True(true);
        }
    }
}