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
                db => db.SetAddValue(RedisPassportsRepository.PassportKeys, "1111-2222"),
                Times.Once()
            );
            _mockRedisDatabase.Verify(
                db => db.StringAddObject<Passport>("1111-2222", passport),
                Times.Once()
            );
        }

        [Test]
        public void Update_AddPassportHistoryAndKey()
        {
            Passport passport = new Passport()
            {
                Series = 1111,
                Number = 2222,
                History = new List<PassportHistory>{
                        new PassportHistory(){ ChangeType = PassportStatus.Add },
                        new PassportHistory(){ ChangeType = PassportStatus.Active, DateTimeChange = new DateTime(2020, 10, 10, 0,0,0) }
                    }
            };

            _repository.Add(
                passport
            );

            _mockRedisDatabase.Verify(
                db => db.SetAddValue(RedisPassportsRepository.DateKeys, "1111-2222 - 10.10.2020 0:00:00"),
                Times.Once()
            );
            _mockRedisDatabase.Verify(
                db => db.StringAddObject<PassportHistory>("1111-2222 - 10.10.2020 0:00:00", passport.History.Last()),
                Times.Once()
            );
        }
    }
}
