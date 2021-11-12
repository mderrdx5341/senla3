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
        private Mock<IRedisDataBase> _mockRedisDatabase;
        private RedisPassportsRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _mockRedisDatabase = new Mock<IRedisDataBase>();
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
                        new PassportHistory(){ ChangeType = PassportStatus.Add, DateTimeChange = new DateTime(2020, 10, 10, 0,0,0) }
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

            _repository.Update(
                passport
            );

            _mockRedisDatabase.Verify(
                db => db.SetAddValue("dates", "1111-2222 - 10.10.2020 0:00:00"),
                Times.Once()
            );
            _mockRedisDatabase.Verify(
                db => db.StringAddObject<PassportHistory>("1111-2222 - 10.10.2020 0:00:00", passport.History.Last()),
                Times.Once()
            );
        }

        [Test]
        public void GetAll_CallKeyMethods()
        {
            RedisKey[] redisKeys = new []{ new RedisKey("1111-2222"), new RedisKey("1111-3333")};
            _mockRedisDatabase.Setup(
                db => db.SetGetValuesAsKeys("passports")
            ).Returns(redisKeys);

            _repository.GetAll();

            _mockRedisDatabase.Verify(
                db => db.StringGetObjects<Passport>(redisKeys),
                Times.Once()
            );
        }
    }
}
