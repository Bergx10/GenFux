﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GenFu.Tests
{
    public class When_configuring_property_with_custom_filler
    {
        public When_configuring_property_with_custom_filler()
        {
            A.Reset();
        }

        [Fact]
        public void string_property_should_be_set_to_custom_filled_value()
        {
            A.Configure<BlogPost>().Fill(b => b.Body, b => $"Body for {b.Title}");

            var bunchOfPosts = A.ListOf<BlogPost>(1000);
            Assert.Equal(1000, bunchOfPosts.Count);
            foreach (var post in bunchOfPosts)
            {
                Assert.Equal("Body for " + post.Title, post.Body);
            }
        }
    }
}
