﻿using System;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Toggl.Multivac;
using Toggl.Multivac.Models;
using Toggl.Ultrawave.ApiClients;
using Toggl.Ultrawave.Exceptions;
using Toggl.Ultrawave.Network;
using Toggl.Ultrawave.Tests.Integration.BaseTests;
using Xunit;

namespace Toggl.Ultrawave.Tests.Integration
{
    public sealed class UserApiTests
    {
        public sealed class TheGetMethod : AuthenticatedEndpointBaseTests<IUser>
        {
            protected override IObservable<IUser> CallEndpointWith(ITogglApi togglApi)
                => togglApi.User.Get();

            [Fact, LogTestInfo]
            public async Task ReturnsValidEmail()
            {
                var (email, password) = await User.CreateEmailPassword();
                var credentials = Credentials.WithPassword(email, password);
                var api = TogglApiWith(credentials);

                var user = await api.User.Get();
                user.Email.Should().Be(email.ToString());
            }

            [Fact, LogTestInfo]
            public async Task ReturnsValidId()
            {
                var (togglApi, user) = await SetupTestUser();

                var userFromApi = await CallEndpointWith(togglApi);

                userFromApi.Id.Should().NotBe(0);
            }

            [Fact, LogTestInfo]
            public async Task ReturnsValidApiToken()
            {
                var (togglApi, user) = await SetupTestUser();
                var regex = "^[a-fA-F0-9]+$";

                var userFromApi = await CallEndpointWith(togglApi);

                userFromApi.ApiToken.Should().NotBeNull()
                           .And.HaveLength(32)
                           .And.MatchRegex(regex);
            }

            [Fact, LogTestInfo]
            public async Task ReturnsValidDateFormat()
            {
                var (togglApi, user) = await SetupTestUser();
                string[] validFormats =
                {
                    "%m/%d/%Y",
                    "%d-%m-%Y",
                    "%m-%d-%Y",
                    "%Y-%m-%d",
                    "%d/%m/%Y",
                    "%d.%m.%Y"
                };

                var userFromApi = await CallEndpointWith(togglApi);

                userFromApi.DateFormat.Should().BeOneOf(validFormats);
            }

            [Fact, LogTestInfo]
            public async Task ReturnsValidBeginningOfWeek()
            {
                var (togglApi, user) = await SetupTestUser();

                var userFromApi = await CallEndpointWith(togglApi);
                var beginningOfWeekInt = (int)userFromApi.BeginningOfWeek;

                beginningOfWeekInt.Should().BeInRange(0, 6);
            }

            [Fact, LogTestInfo]
            public async Task ReturnsValidDefaultWorkspaceId()
            {
                var (togglApi, user) = await SetupTestUser();

                var userFromApi = await CallEndpointWith(togglApi);
                var workspace = await togglApi.Workspaces.GetById(userFromApi.DefaultWorkspaceId);

                userFromApi.DefaultWorkspaceId.Should().NotBe(0);
                workspace.Should().NotBeNull();
            }

            [Fact, LogTestInfo]
            public async Task ReturnsValidImageUrl()
            {
                var (togglApi, user) = await SetupTestUser();

                var userFromApi = await CallEndpointWith(togglApi);

                userFromApi.ImageUrl.Should().NotBeNullOrEmpty();
                var uri = new Uri(userFromApi.ImageUrl);
                var uriIsAbsolute = uri.IsAbsoluteUri;
                uriIsAbsolute.Should().BeTrue();
            }
        }
        
        public class TheSignUpMethod : EndpointTestBase
        {
            private readonly ITogglApi unauthenticatedTogglApi;

            public TheSignUpMethod()
            {
                unauthenticatedTogglApi = TogglApiWith(Credentials.None);
            }

            [Theory, LogTestInfo]
            [InlineData(null)]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData("\t")]
            [InlineData(" \t ")]
            [InlineData("\n")]
            [InlineData(" \n ")]
            [InlineData(" \t\n ")]
            [InlineData("           ")]
            public void FailsWhenTheEmailIsEmpty(string empty)
            {
                Action signingUp = () => unauthenticatedTogglApi.User.SignUp(createInvalidEmail(empty), "dummyButValidPassword").Wait();

                signingUp.ShouldThrow<BadRequestException>();
            }

            [Theory, LogTestInfo]
            [InlineData("not an email")]
            [InlineData("em@il")]
            [InlineData("domain.com")]
            [InlineData("thisIsNotAnEmail@.com")]
            [InlineData("@email.com")]
            [InlineData("alsoNot@email.")]
            [InlineData("double@at@email.com")]
            [InlineData("so#close@yet%so.far")]
            public void FailsWhenTheEmailIsNotValid(string invalidEmail)
            {
                Action signingUp = () => unauthenticatedTogglApi.User.SignUp(createInvalidEmail(invalidEmail), "dummyButValidPassword").Wait();

                signingUp.ShouldThrow<BadRequestException>();
            }

            [Theory, LogTestInfo]
            [InlineData("")]
            [InlineData(" ")]
            [InlineData("\t")]
            [InlineData(" \t ")]
            [InlineData("\n")]
            [InlineData(" \n ")]
            [InlineData(" \t\n ")]
            [InlineData("           ")]
            public void FailsWhenThePasswordIsEmpty(string empty)
            {
                Action signingUp = () => unauthenticatedTogglApi.User.SignUp(Email.FromString("dummy@email.com"), empty).Wait();

                signingUp.ShouldThrow<BadRequestException>();
            }

            [Fact]
            public async Task CreatesANewUserAccount()
            {
                var emailAddress = Email.FromString($"{Guid.NewGuid().ToString()}@address.com");

                var user = await unauthenticatedTogglApi.User.SignUp(emailAddress, "somePassword");

                user.Email.Should().Be(emailAddress.ToString());
            }

            [Fact]
            public async Task FailsWhenTheEmailIsAlreadTaken()
            {
                var email = Email.FromString($"{Guid.NewGuid().ToString()}@address.com");
                var firstUser = await unauthenticatedTogglApi.User.SignUp(email, "somePassword");

                Action secondSigningUp = () => unauthenticatedTogglApi.User.SignUp(email, "thePasswordIsNotImportant").Wait();

                secondSigningUp.ShouldThrow<BadRequestException>();
            }

            [Fact]
            public async Task EnablesLoginForTheNewlyCreateUserAccount()
            {
                var emailAddress = Email.FromString($"{Guid.NewGuid().ToString()}@address.com");
                var password = Guid.NewGuid().ToString();

                var signedUpUser = await unauthenticatedTogglApi.User.SignUp(emailAddress, password);
                var credentials = Credentials.WithPassword(emailAddress, password);
                var togglApi = TogglApiWith(credentials);
                var user = await togglApi.User.Get();

                signedUpUser.Id.Should().Be(user.Id);
            }

            private Email createInvalidEmail(string invalidEmailAddress)
            {
                var constructor = typeof(Email).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0];
                return (Email)constructor.Invoke(new object[] { invalidEmailAddress });
            }
        }
    }
}
