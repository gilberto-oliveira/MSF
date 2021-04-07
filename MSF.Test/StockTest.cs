using MSF.Domain.Context;
using MSF.Domain.UnitOfWork;
using MSF.Service.Stock;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using MSF.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MSF.Test
{
    [TestFixture]
    public class StockTest
    {
        private StockService _stockService;

        [SetUp]
        public void Setup()
        {
            var configuration = ConfigurationFactory.GetConfiguration();

            var builder = new DbContextOptionsBuilder<MSFDbContext>();

            var connection = configuration.GetConnectionString("MSF_DEV");

            builder.UseSqlServer(connection);

            _stockService = new StockService(new UnitOfWork(new MSFDbContext(builder.Options, null)));
        }

        [Test]
        public async Task ProductList_Return3Products()
        {
            var produts = await _stockService.FindProductByFilter("");
            Assert.AreEqual(3, produts.Take(3).Count());
        }

        [Test]
        public async Task ProviderList_Return1Provider()
        {
            var providers = await _stockService.FindProviderByFilterAndProduct("", 7);
            Assert.AreEqual(1, providers.Take(1).Count());
        }

        [Test]
        public async Task FindTotalPriceByProductAndProvider_Product5Provider7Return115200M()
        {
            var val = await _stockService.FindTotalPriceByProductAndProvider(5, 7);
            Assert.AreEqual(11.5200M, val);
        }
    }
}