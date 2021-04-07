using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MSF.Common;
using MSF.Domain.Context;
using MSF.Domain.UnitOfWork;
using MSF.Service.WorkCenter;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace MSF.Test
{
    [TestFixture]
    public class WorkCenterTest
    {
        private WorkCenterService _workCenterService;

        [SetUp]
        public void Setup()
        {
            var configuration = ConfigurationFactory.GetConfiguration();

            var builder = new DbContextOptionsBuilder<MSFDbContext>();

            var connection = configuration.GetConnectionString("MSF_DEV");

            builder.UseSqlServer(connection);

            _workCenterService = new WorkCenterService(null, new UnitOfWork(new MSFDbContext(builder.Options, null)));
        }

        [Test]
        public async Task WorkCenterList_ReturnOneWorkCenter()
        {
            var workCenters = await _workCenterService.FindByShopAsync(1);
            Assert.AreEqual(1, workCenters.Count());
        }


    }
}
