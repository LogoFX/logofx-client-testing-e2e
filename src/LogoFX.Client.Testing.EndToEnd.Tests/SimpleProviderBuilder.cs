﻿using System.Collections.Generic;
using Attest.Fake.Setup.Contracts;
using LogoFX.Client.Data.Fake.ProviderBuilders;

namespace LogoFX.Client.Testing.EndToEnd.Tests
{
    public class SimpleItemDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }

    public interface ISimpleProvider
    {
        IEnumerable<SimpleItemDto> GetSimpleItems();
    }

    class SimpleProviderBuilder : FakeBuilderBase<ISimpleProvider>
    {
        private readonly List<SimpleItemDto> _warehouseItemsStorage = new List<SimpleItemDto>();

        private SimpleProviderBuilder()
        {

        }

        public static SimpleProviderBuilder CreateBuilder()
        {
            return new SimpleProviderBuilder();
        }

        public void WithWarehouseItems(IEnumerable<SimpleItemDto> warehouseItems)
        {
            _warehouseItemsStorage.Clear();
            _warehouseItemsStorage.AddRange(warehouseItems);
        }       

        protected override IServiceCall<ISimpleProvider> CreateServiceCall(IHaveNoMethods<ISimpleProvider> serviceCallTemplate)
        {
            var setup = serviceCallTemplate
               .AddMethodCallWithResult(t => t.GetSimpleItems(), r => r.Complete(GetSimpleItems));
            return setup;
        }

        private IEnumerable<SimpleItemDto> GetSimpleItems()
        {
            return _warehouseItemsStorage;
        }
    }
}
