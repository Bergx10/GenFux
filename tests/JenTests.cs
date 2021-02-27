﻿using Angela.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angela.Tests
{
    [TestFixture]
    class JenTests
    {
        [Test]
        public void MakeDateRuleFutureIsCorrect()
        {
            Angie.Reset();
            var date = Jen.Date(DateRules.FutureDates);
            Assert.Greater(date, DateTime.Now);
        }

        [Test]
        public void MakeDateRulePastIsCorrect()
        {
            Angie.Reset();
            var date = Jen.Date(DateRules.PastDate);
            Assert.Greater(DateTime.Now, date);
        }

        [Test]
        public void AddressContainsNumbers()
        {
            var addressLine = Jen.AddressLine();

            var streetNumber = 0;
            var addressPrefix = addressLine.Split(' ')[0];

            Assert.IsTrue(int.TryParse(addressPrefix, out streetNumber));

        }


        [Test]
        public void PhoneNumberIsExpectedLength()
        {
            for (int i = 0; i < 1000; i++)
            {
                var phoneNumber = Jen.PhoneNumber();
                Assert.AreEqual(14, phoneNumber.Length);
            }
        }
    }
}
