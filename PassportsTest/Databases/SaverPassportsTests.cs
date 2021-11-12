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
        internal class AssertValues
        {
            public OperationRepository OperationRepository{ get; set; }
            public bool PassportStatus { get; set; }
            public PassportStatus HistoryStatus { get; set; }
        }

        private static object[] Passports =
        {
            //Add
            new object[] {
                new List<Passport>(),
                new List<Passport>()
                {
                    new Passport{ Series=1111, Number=2222},
                },
                new AssertValues(){ OperationRepository = OperationRepository.Add, PassportStatus = false, HistoryStatus = PassportStatus.Add }
            },

            //activated
            new object[] {
                new List<Passport>() {
                        new Passport{ Id=1, IsActive = false, Series=1111, Number=2222},
                },
                new List<Passport>(),
                new AssertValues(){ OperationRepository = OperationRepository.Update, PassportStatus = true, HistoryStatus = PassportStatus.Active }
            },

            //deactivated
            new object[] {
                new List<Passport>() {
                    new Passport{ Id=0, IsActive = true, Series=1111, Number=2222},
                },
                new List<Passport>()
                {
                    new Passport{ Series=1111, Number=2222},
                },
                new AssertValues(){ OperationRepository = OperationRepository.Update, PassportStatus = false, HistoryStatus = PassportStatus.NotActive }
            }
        };

        [TestCaseSource(nameof(Passports))]
        public void ChangeForRepository_PassportAdd_Test(List<Passport> passportInRepository, List<Passport> newPassports, AssertValues assertValues)
        {
            SaverPassports sp = new SaverPassports();
            Dictionary<Passport, OperationRepository> passportForRepository = sp.ChangeForRepository(passportInRepository, newPassports);

            foreach (KeyValuePair<Passport, OperationRepository> passportEntry in passportForRepository)
            {
                Assert.AreEqual(passportEntry.Value, assertValues.OperationRepository);
                Assert.AreEqual(passportEntry.Key.IsActive, assertValues.PassportStatus);
                Assert.AreEqual(passportEntry.Key.History.Last().ChangeType, assertValues.HistoryStatus);
            }
        }
    }
}
