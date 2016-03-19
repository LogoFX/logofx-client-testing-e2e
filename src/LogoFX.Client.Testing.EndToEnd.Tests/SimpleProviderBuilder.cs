﻿using System;
using System.Collections.Generic;
using Attest.Fake.Builders;
using Attest.Fake.Setup;

namespace LogoFX.Client.Testing.EndToEnd.Tests
{
    [Serializable]
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

    [Serializable]
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

        protected override void SetupFake()
        {
            var initSetup = ServiceCall<ISimpleProvider>.CreateServiceCall(FakeService);

            var setup = initSetup
                .AddMethodCallWithResult(t => t.GetSimpleItems(), r => r.Complete(GetSimpleItems));

            setup.Build();
        }

        private IEnumerable<SimpleItemDto> GetSimpleItems()
        {
            return _warehouseItemsStorage;
        }
    }
}
