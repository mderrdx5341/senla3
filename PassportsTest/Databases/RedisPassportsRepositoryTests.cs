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
        public void Add_AddPassportAndKey()
        {
            Passport passport = new Passport()
            {
                Series = 1111,
                Number = 2222,
                History = new List<PassportHistory>{
                        new PassportHistory(){ ChangeType = PassportStatus.Add }
                    }
            };

            _repository.Add(
                passport
            );

            _mockRedisDatabase.Verify(
                db => db.SetAddValue("passports", "1111-2222"),
                Times.Once()
            );
            _mockRedisDatabase.Verify(
                db => db.StringAddObject<Passport>("1111-2222", passport),
                Times.Once()
            );
        }
    }
}
