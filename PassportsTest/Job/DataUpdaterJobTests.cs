using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Passports.Jobs;
using Passports.Models;
using Moq;
using Microsoft.Extensions.Configuration;

namespace PassportsTest.Job
{    
    [TestFixture]
    class DataUpdaterJobTests
    {
        private Mock<IPassportsRepository> _mockRepository;
        private Mock<IConfiguration> _configuration;
        private DataUpdaterJob _dataUpdaterJob;

        private StringBuilder GetCSVContent()
        {
            StringBuilder csvFile = new StringBuilder();
            csvFile.AppendLine("1111;2222");
            csvFile.AppendLine("1111;3333");

            return csvFile;
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IPassportsRepository>(MockBehavior.Strict);
            _configuration = new Mock<IConfiguration>();
            _dataUpdaterJob = new DataUpdaterJob(_mockRepository.Object, _configuration.Object);
        }

        [Test]
        public void UpdateData_readCSV()
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(GetCSVContent().ToString()));
            List<Passport> passports = _dataUpdaterJob.ReadDataFromCSV(ms);

            Assert.AreEqual(passports.Count, 2);
            Assert.AreEqual(passports.Last().Number, 3333);
        }
    }
}
