using Moq;
using NUnit.Framework;
using Passports.Models;
using Passports.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassportsTest.Services
{
    [TestFixture]
    internal class PassportsServiceTests
    {
        private Mock<IPassportsRepository> _mockPassportRepository;
        private PassportsService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _mockPassportRepository = new Mock<IPassportsRepository>(MockBehavior.Strict);
            _service = new PassportsService(_mockPassportRepository.Object);
        }

        [Test]
        public void GetAll_Test()
        {
            _mockPassportRepository.Setup(r => r.GetAll()).Returns(
                    new List<Passport>() { 
                        new Passport() { Id=1, IsActive=false, Series=1111, Number=2222},
                        new Passport() { Id=1, IsActive=false, Series=1111, Number=3333},
                        new Passport() { Id=1, IsActive=false, Series=1111, Number=4444}
                    }
                );
            List<Passport> lp = _service.GetPassports();
            Assert.AreEqual(lp.Count, 3);
        }
    }
}
