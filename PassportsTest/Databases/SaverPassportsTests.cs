using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Passports.DataBases;
using Passports.Models;


namespace PassportsTest.Databases
{
    [TestFixture]
    internal class SaverPassportsTests
    {
        [Test]
        public void ChangeForRepository_PassportAdd()
        {
            List<Passport> passportInRepository = new List<Passport>();
            List<Passport> newPassports = new List<Passport>()
            {
                new Passport{ Series=1111, Number=2222},
            };

            SaverPassports sp = new SaverPassports();
            Dictionary<Passport, OperationRepository> passportForRepository = sp.ChangeForRepository(passportInRepository, newPassports);

            foreach (KeyValuePair<Passport, OperationRepository> passportEntry in passportForRepository)
            {
                Assert.AreEqual(passportEntry.Value, OperationRepository.Add);
                Assert.IsFalse(passportEntry.Key.IsActive);
                Assert.AreEqual(passportEntry.Key.History.Last().ChangeType, PassportStatus.Add);
            }
        }

        [Test]
        public void ChangeForRepository_PassportActivation()
        {
            List<Passport> passportInRepository = new List<Passport>() {
                    new Passport{ Id=1, IsActive = false, Series=1111, Number=2222},
            };
            List<Passport> newPassports = new List<Passport>();

            SaverPassports sp = new SaverPassports();
            Dictionary<Passport, OperationRepository> passportForRepository = sp.ChangeForRepository(passportInRepository, newPassports);

            foreach (KeyValuePair<Passport, OperationRepository> passportEntry in passportForRepository)
            {
                Assert.AreEqual(passportEntry.Value, OperationRepository.Update);
                Assert.IsTrue(passportEntry.Key.IsActive);
                Assert.AreEqual(passportEntry.Key.History.Last().ChangeType, PassportStatus.Active);
            }
        }

        [Test]
        public void ChangeForRepository_PassportDeactivation()
        {
            List<Passport> passportInRepository = new List<Passport>() {
                new Passport{ Id=0, IsActive = true, Series=1111, Number=2222},
            };
            List<Passport> newPassports = new List<Passport>()
            {
                new Passport{ Series=1111, Number=2222},
            };

            SaverPassports sp = new SaverPassports();
            Dictionary<Passport, OperationRepository> passportForRepository = sp.ChangeForRepository(passportInRepository, newPassports);

            foreach (KeyValuePair<Passport, OperationRepository> passportEntry in passportForRepository)
            {
                Assert.AreEqual(passportEntry.Value, OperationRepository.Update);
                Assert.IsFalse(passportEntry.Key.IsActive);
                Assert.AreEqual(passportEntry.Key.History.Last().ChangeType, PassportStatus.NotActive);
            }
        }
    }
}
