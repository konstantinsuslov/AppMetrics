﻿// <copyright file="KeyValuePairMetricsOptionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Options
{
    public class KeyValuePairMetricsOptionsTests
    {
        [Fact]
        public void Can_load_options_from_key_values()
        {
            // Arrange
            var keyValuePairs = new Dictionary<string, string>
                          {
                              { "AddDefaultGlobalTags", "false" },
                              { "DefaultContextLabel", "Testing" },
                              { "GlobalTags", "tag1=value1, tag2=value2" },
                              { "MetricsEnabled", "false" }
                          };

            // Act
            var options = new KeyValuePairMetricsOptions(keyValuePairs).AsOptions();

            // Assert
            options.AddDefaultGlobalTags.Should().BeFalse();
            options.DefaultContextLabel.Should().Be("Testing");
            options.GlobalTags.Count.Should().Be(2);
            options.GlobalTags.First().Key.Should().Be("tag1");
            options.GlobalTags.First().Value.Should().Be("value1");
            options.GlobalTags.Skip(1).First().Key.Should().Be("tag2");
            options.GlobalTags.Skip(1).First().Value.Should().Be("value2");
            options.MetricsEnabled.Should().BeFalse();
        }

        [Fact]
        public void Key_value_pairs_cannot_be_null()
        {
            // Arrange
            // Act
            Action action = () =>
            {
                var options = new KeyValuePairMetricsOptions(null);
            };

            // Assert
           action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Add_default_global_tags_should_be_bool()
        {
            // Arrange
            // Act
            Action action = () =>
            {
                var keyValuePairs = new Dictionary<string, string>
                                    {
                                        { "AddDefaultGlobalTags", "not_a_bool" }
                                    };

                // Act
                var options = new KeyValuePairMetricsOptions(keyValuePairs).AsOptions();
            };

            // Assert
            action.ShouldThrow<InvalidCastException>();
        }

        [Fact]
        public void Metrics_enabled_should_be_bool()
        {
            // Arrange
            // Act
            Action action = () =>
            {
                var keyValuePairs = new Dictionary<string, string>
                                    {
                                        { "MetricsEnabled", "not_a_bool" }
                                    };

                // Act
                var options = new KeyValuePairMetricsOptions(keyValuePairs).AsOptions();
            };

            // Assert
            action.ShouldThrow<InvalidCastException>();
        }

        [Theory]
        [InlineData("invalidseperator|value1, tag2=value2")]
        [InlineData("no_comma")]
        public void Global_tags_should_be_formatted_correctly(string tags)
        {
            // Arrange
            // Act
            Action action = () =>
            {
                var keyValuePairs = new Dictionary<string, string>
                                    {
                                        { "GlobalTags", tags }
                                    };

                // Act
                var options = new KeyValuePairMetricsOptions(keyValuePairs).AsOptions();
            };

            // Assert
            action.ShouldThrow<InvalidOperationException>();
        }
    }
}
