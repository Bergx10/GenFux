﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Angela.Core.ValueGenerators.People
{
    public class Names : BaseValueGenerator
    {
        public static string Title()
        {
            return GetRandomValue(Susan.Data(Properties.Titles));
        }

        public static string LastName()
        {
            return GetRandomValue(Susan.Data(Properties.LastNames));
        }

        public static string FirstName()
        {
            return GetRandomValue(Susan.Data(Properties.FirstNames));
        }

        public static string UserName()
        {
            return FirstName().First() + LastName();
        }

        public static string FullName()
        {
            return String.Format("{0} {1}", FirstName(), LastName());
        }

    }
}
