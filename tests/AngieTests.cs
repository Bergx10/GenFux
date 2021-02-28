﻿using System.Text.RegularExpressions;
using Angela.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angela.Tests
{
    [TestFixture]
    class AngieTests
    {
        [SetUp]
        public void ResetAngie()
        {
            Angie.Reset();
        }

        [Test]
        public void StringInNewClassIsPopulated()
        {
            var person = Angie.FastMake<Person>();
            Assert.IsTrue(!string.IsNullOrEmpty(person.FirstName));
        }

        [Test]
        public void EmailAddressInNewClassIsValid()
        {
            Person person = Angie.FastMake<Person>();
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(person.EmailAddress);
            Assert.IsTrue(match.Success, "Invalid Email Address: {0}", person.EmailAddress);

        }

        [Test]
        public void PhoneNumberInNewClassIsPopulated()
        {
            var person = Angie.FastMake<Person>();
            Assert.IsTrue(!string.IsNullOrEmpty(person.PhoneNumber));
        }

        [Test]
        public void IntInNewClassIsPopulated()
        {
            var person = Angie.FastMake<Person>();
            Assert.IsTrue(person.Age != default(int));
        }

        [Test]
        public void BasicTypesInExistingClassArePopulated()
        {
            var person = Angie.FastFill(new Person());

            // for test brievity
            Assert.IsTrue(!string.IsNullOrEmpty(person.FirstName), "String property was not populated. Aborting additional asserts in test.");
            Assert.IsTrue(person.Age != default(int), "Int property was not populated. Aborting additional asserts in test.");
            Assert.IsTrue(person.BirthDate != default(DateTime), "DateTime was left as default value. Aborting additional asserts in test.");
        }

        [Test]
        public void PopulatedBasicTypesInExistingClassRemainsUnchanged()
        {
            var firstName = "Angie";
            var age = 29;
            var date = DateTime.Now.AddYears(-29);
            var person = Angie.FastFill(new Person { FirstName = firstName, Age = age, BirthDate = date });

            // for test brievity
            Assert.AreEqual(person.FirstName, firstName, "String property was altered. Aborting additional asserts in test.");
            Assert.AreEqual(person.Age, age, "Int property was altered. Aborting additional asserts in test.");
            Assert.AreEqual(person.BirthDate, date, "Date was altered. Aborting additional asserts in test.");
        }

        [Test]
        public void IntMaxNotExceededOnGeneratedValue()
        {
            var maxNumberOfKids = 5;

            for (int i = 0; i < 2500; i++)
            {
                var person = Angie
                    .Configure()
                    .MaxInt(maxNumberOfKids)
                    .Make<Person>();

                if (!(person.NumberOfKids <= maxNumberOfKids))
                    Assert.Fail("Int max was exceeded: {0} ", person.NumberOfKids);
            }

        }
        
        [Test]
        public void IntMinNotExceededOnGeneratedValue()
        {
            var minNumberOfKids = 5;

            for (int i = 0; i < 2500; i++)
            {
                var person = Angie
                    .Configure()
                    .MinInt(minNumberOfKids)
                    .Make<Person>();

                if (!(person.NumberOfKids >= minNumberOfKids))
                    Assert.Fail("Int min was exceeded: {0} ", person.NumberOfKids);
            }

            

        }
        
        [Test]
        public void IntRangeWithinBoundsOnGeneratedValue()
        {
            var success = true;

            // use a small window to try to force collisions
            var minAge = 20;
            var maxAge = 22;

            Angie.Reset();

            for (int i = 0; i < 1000; i++)
            {
                var person = Angie                    
                    .Configure<Person>()
                    .FillProperty(p=> p.Age)
                    .IntRange(minAge, maxAge)
                    .Make<Person>();

                success = (person.Age >= minAge && person.Age <= maxAge);

                Assert.IsTrue(success, "Int was generated outside of range.{0}", person.Age);
            }            

        }

        [Test]
        public void AgeIsAlwaysPositive()
        {
            var person = Angie.FastMake<Person>();
            Assert.IsTrue(person.Age >= 0);
        }

        [Test]
        public void FastListDefaultGenerates25Entries()
        {
            var people = Angie.FastList<Person>();

            Assert.AreEqual(Angie.Defaults.LIST_COUNT, people.Count(),
                string.Format("Expected {0} but collection contained {1}", 
                Angie.Defaults.LIST_COUNT, people.Count())
                );
        }

        [Test]
        public void FastListGeneratesCorrectNumberOfEntries()
        {
            var personCount = 17;
            var people = Angie.FastList<Person>(personCount);
            Assert.AreEqual(people.Count(), personCount);
        }

        [Test]
        public void MakeListDefaultsTo25Entries()
        {
            var people = Angie
                .Configure()                // get an instantiated object for the non-static call
                .MakeList<Person>();
            Assert.IsTrue(people.Count() == Angie.Defaults.LIST_COUNT);
        }

        [Test]
        public void MakeListGeneratesCorrectNumberOfEntries()
        {
            var personCount = 13;
            var people = Angie
                .Configure()
                .ListCount(personCount)
                .MakeList<Person>();
            Assert.IsTrue(people.Count() == personCount);
        }

        [Test]
        public void DateTimesAreInitialized()
        {
            var post = Angie.FastMake<BlogPost>();
            Assert.IsTrue(post.CreateDate != default(DateTime));
        }

        [Test]
        public void DateTimesStayWithinConfiguredDates()
        {
            var success = true;

            // use a small window to try to force collisions
            var minDate = DateTime.Now.AddMilliseconds(-5);
            var maxDate = DateTime.Now.AddMilliseconds(5);

            for (int i = 0; i < 1000; i++)
            {
                var person = Angie
                    .Configure()
                    .DateRange(DateTime.Now.AddMilliseconds(-10), DateTime.Now.AddMilliseconds(10))
                    .Make<Person>();

                if (!(person.BirthDate >= minDate && person.BirthDate <= maxDate))
                    success = false;
                else
                    Assert.IsTrue(success, "Date was generated outside of range.{0}", person.BirthDate);
            }


        }

        [Test]
        public void IntPropertyFilledBySpecificMethod()
        {
            var age = 11;

            var person = Angie.Configure<Person>()
                .FillBy(p => p.Age, delegate() { return age; })
                .Make<Person>();

            Assert.IsTrue(person.Age == age);

        }

        [Test]
        public void StringPropertyFilledBySpecificMethod()
        {
            var blogTitle = "Angie";

            var post = Angie.Configure<BlogPost>()
                .FillBy(b => b.Title, () => blogTitle)
                .Make<BlogPost>();

            Assert.AreEqual(blogTitle, post.Title);
        }

        [Test]
        public void DateTimeFilledWithExpectedDateGivenRules()
        {
            var future = DateTime.Now.AddSeconds(1);

            var comments = Angie
                .Configure<BlogComment>()                
                .FillBy(b => b.CommentDate, delegate() { return Jen.FillDate(DateRules.FutureDates); })
                .ListCount(1000)
                .MakeList<BlogComment>();

            foreach (var comment in comments)
            {
                Assert.Greater(comment.CommentDate, future);
            }
        }

        [Test]
        public void ComplexPropertyFillsExecuted()
        {
            var postcomments = Angie
            .Configure()
            .ListCount(5)
            .MakeList<BlogComment>();

            var blogpost = Angie
                .Configure<BlogPost>()
                .FillBy(b => b.Comments, delegate { return postcomments; })
                .Make<BlogPost>();

            Assert.IsNotNull(blogpost.Comments);

        }

        [Test]
        public void CanadianProvinceIsFilled()
        {
            var location = Angie.FastMake<CanadianLocation>();
            Assert.IsFalse(string.IsNullOrEmpty(location.Province));
        }

        [Test]
        public void UsaStateIsFilled()
        {
            var location = Angie.FastMake<AmericanLocation>();
            Assert.IsFalse(string.IsNullOrEmpty(location.State));
        }

        [Test]
        public void CustomPropertyFillsAreChainableUsingSet()
        {
            var blogpost = Angie
                .Configure<BlogPost>()
                .FillBy(b => b.CreateDate, delegate() { return Jen.FillDate(DateRules.PastDate); })
                .FillBy(b => b.Comments, delegate()
                {
                    return Angie
                        .Set<BlogComment>()                        
                        .FillBy(b => b.CommentDate, delegate() { return Jen.FillDate(DateRules.PastDate); })
                        .ListCount(5)
                        .MakeList<BlogComment>();
                })
            .Make<BlogPost>();

            Assert.IsNotNull(blogpost.Comments);
        }
    }
}
