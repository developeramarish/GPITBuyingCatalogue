﻿using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using Xunit;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Authorization
{
    public sealed class Login : TestBase, IClassFixture<LocalWebApplicationFactory>, IDisposable
    {
        public Login(LocalWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public void Login_FormDisplayed()
        {
            PublicBrowsePages.CommonActions.ClickLoginLink();

            AuthorizationPages.LoginActions.EmailAddressInputDisplayed().Should().BeTrue();
            AuthorizationPages.LoginActions.PasswordInputDisplayed().Should().BeTrue();
            AuthorizationPages.LoginActions.LoginButtonDisplayed().Should().BeTrue();
        }

        [Fact]
        public async Task Login_LoginSuccessful()
        {
            using var context = GetUsersContext();
            var userEmail = (await context.AspNetUsers.FirstAsync(s => s.OrganisationFunction == "Authority")).Email;

            PublicBrowsePages.CommonActions.ClickLoginLink();

            AuthorizationPages.LoginActions.Login(userEmail, DefaultPassword);

            AuthorizationPages.CommonActions.LogoutLinkDisplayed().Should().BeTrue();
        }

        [Theory]
        [InlineData("user", "")]
        [InlineData("user", "falsePassword")]
        [InlineData("falseUser@email.com", "password")]
        [InlineData("", "password")]
        public async Task Login_UnsuccessfulLogin(string user, string password)
        {
            using var context = GetUsersContext();
            var userEmail = user == "user" ? (await context.AspNetUsers.FirstAsync(s => s.OrganisationFunction == "Authority")).Email : user;
            var userPassword = password == "password" ? DefaultPassword : password;

            PublicBrowsePages.CommonActions.ClickLoginLink();

            AuthorizationPages.LoginActions.Login(userEmail, userPassword);

            AuthorizationPages.CommonActions.LogoutLinkDisplayed().Should().BeFalse();

            AuthorizationPages.LoginActions.EmailAddressInputDisplayed().Should().BeTrue();
            AuthorizationPages.LoginActions.PasswordInputDisplayed().Should().BeTrue();
            AuthorizationPages.LoginActions.LoginButtonDisplayed().Should().BeTrue();
        }
        

        public void Dispose()
        {
            // Force logout at end of test
            driver.Manage().Cookies.DeleteAllCookies();
        }
    }
}