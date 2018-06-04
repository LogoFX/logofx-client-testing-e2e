﻿using System;
using System.Collections.Generic;
using Attest.Fake.Setup.Contracts;
using LogoFX.Client.Data.Fake.ProviderBuilders;
using Moq;

namespace LogoFX.Client.Testing.EndToEnd.Tests
{
    public interface IAnotherProvider
    {
        void Login(string username, string password);
        IEnumerable<string> GetUsers();
    }

    public class AnotherProviderBuilder : FakeBuilderBase<IAnotherProvider>
    {
        private readonly Dictionary<string, string> _users = new Dictionary<string, string>();

        private AnotherProviderBuilder()
        {

        }

        public static AnotherProviderBuilder CreateBuilder()
        {
            return new AnotherProviderBuilder();
        }

        public void WithUser(string username, string password)
        {
            _users.Add(username, password);
        }

        protected override IServiceCall<IAnotherProvider> CreateServiceCall(IHaveNoMethods<IAnotherProvider> serviceCallTemplate)
        {
            var setup = serviceCallTemplate
                .AddMethodCall<string, string>(t => t.Login(It.IsAny<string>(), It.IsAny<string>()),
                    (r, login, password) => _users.ContainsKey(login)
                        ? _users[login] == password
                            ? r.Complete()
                            : r.Throw(new Exception("Unable to login."))
                        : r.Throw(new Exception("Login not found.")))
                        .AddMethodCallWithResult(t => t.GetUsers(), r => r.Complete(_users.Keys));
            return setup;
        }
    }
}
