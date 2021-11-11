using Moq;
using NUnit.Framework;
using Passports.DataBases;
using Passports.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassportsTest.Databases
{
    [TestFixture]
    class RedisPassportsRepositoryTest
    {
        private Mock<RedisDataBase> _mockRedisDatabase;
        private RedisPassportsRepository _repository;

        [OneTimeSetUp]
        public void SetUp()
        {
            Mock<IConnectionMultiplexer> _cm = new Mock<IConnectionMultiplexer>();
            _mockRedisDatabase = new Mock<RedisDataBase>(_cm.Object);
            _repository = new RedisPassportsRepository(_mockRedisDatabase.Object, new SaverPassports());
        }

        [Test]
        public void AddPassport_Test()
        {
            _mockRedisDatabase.Setup(
                db => db.SetAddValue(It.IsAny<string>(), It.IsAny<string>())
            );

            _mockRedisDatabase.Setup(
                db => db.StringAddObject<Passport>(It.IsAny<string>(), It.IsAny<Passport>())
            );
            _mockRedisDatabase.Setup(
                db => db.SetGetValuesAsKeys(It.IsAny<string>())
            ).Returns(new RedisKey[1]);

            _mockRedisDatabase.Setup(
                db => db.StringGetObjects<Passport>(It.IsAny<RedisKey[]>())
            ).Returns(new List<Passport>() { 
                new Passport(){Series = 1111, Number = 2222, IsActive = false}
            });

            _repository.Add(new Passport()
                {
                    Series = 1111,
                    Number = 2222,
                    History = new List<PassportHistory>{
                        new PassportHistory(){ ChangeType = PassportStatus.Add }
                    }
                }
            );
            List<Passport> lp = _repository.GetAll();
            Assert.AreEqual(lp.Count, 1);
        }
    }
}
