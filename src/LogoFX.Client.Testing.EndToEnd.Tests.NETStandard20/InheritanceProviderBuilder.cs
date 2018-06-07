﻿using System;
using System.Collections.Generic;
using Attest.Fake.Setup.Contracts;
using LogoFX.Client.Data.Fake.ProviderBuilders;

namespace LogoFX.Client.Testing.EndToEnd.Tests
{
    public class DtoBase
    {
        public int Size { get; set; }
        public string Name { get; set; }
    }

    public class InheritanceDto : DtoBase
    {
        public string Kind { get; set; }
        public int Score { get; set; }
    }

    public interface IInheritanceProvider
    {
        InheritanceDto[] GetObjects();
    }

    class InheritanceProviderBuilder : FakeBuilderBase<IInheritanceProvider>
    {
        private readonly List<InheritanceDto> _objects = new List<InheritanceDto>();

        private InheritanceProviderBuilder()
        {

        }

        public static InheritanceProviderBuilder CreateBuilder()
        {
            return new InheritanceProviderBuilder();
        }

        public void WithObjects(IEnumerable<InheritanceDto> inheritance)
        {
            _objects.AddRange(inheritance);
        }

        protected override IServiceCall<IInheritanceProvider> CreateServiceCall(IHaveNoMethods<IInheritanceProvider> serviceCallTemplate)
        {
            var setup = serviceCallTemplate
                .AddMethodCallWithResult(t => t.GetObjects(),
                    r => r.Complete(_objects.ToArray()));
            return setup;
        }
    }
}
