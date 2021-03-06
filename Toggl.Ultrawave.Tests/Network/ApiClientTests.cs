﻿using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Toggl.Ultrawave.Network;
using Xunit;

namespace Toggl.Ultrawave.Tests.Network
{
    public sealed class ApiClientTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void AddsTheUserAgentToTheProvidedHttpClient()
            {
                var userAgent = new UserAgent("Test", "1.0");
                var expectedUserAgent = userAgent.ToString();

                var httpClient = new HttpClient();
                var apiClient = new ApiClient(httpClient, userAgent);

                var actualUserAgent = httpClient.DefaultRequestHeaders.UserAgent.Single().ToString();
                actualUserAgent.Should().Be(expectedUserAgent);
            }
        }
    }
}
