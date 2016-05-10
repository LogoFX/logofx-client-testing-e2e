﻿using System;
using System.Linq;
using Attest.Fake.Builders;
using Attest.Fake.Moq;
using LogoFX.Client.Testing.EndToEnd.FakeData.Shared;
using NUnit.Framework;

namespace LogoFX.Client.Testing.EndToEnd.Tests
{
    [TestFixture]
    class SerializationTests
    {
        [Test]
        public void WhenItemsAreSerializedAndItemsAreDeserialized_ThenItemsCollectionIsCorrect()
        {
            var items = new[]
            {
                new SimpleItemDto
                {
                    Name = "KindOne",
                    Price = 54,
                    Quantity = 3
                },
                new SimpleItemDto
                {
                    Name = "KindTwo",
                    Price = 67,
                    Quantity = 4
                },
                new SimpleItemDto
                {
                    Name = "KindThree",
                    Price = 65,
                    Quantity = 6
                }
            };
            FakeFactoryContext.Current = new FakeFactory();
            var simpleBuilder = SimpleProviderBuilder.CreateBuilder();
            simpleBuilder.WithWarehouseItems(items);

            BuildersCollectionContext.AddBuilder(simpleBuilder);
            BuildersCollectionContext.SerializeBuilders();
            BuildersCollectionContext.DeserializeBuilders();
            var builders = BuildersCollectionContext.GetBuilders<ISimpleProvider>();
            var actualBuilder = builders.First();

            var actualItems = actualBuilder.GetService().GetSimpleItems().ToArray();
            for (int i = 0; i < Math.Max(items.Length, actualItems.Length); i++)
            {
                var item = items[i];
                var actualItem = actualItems[i];
                Assert.AreEqual(item.Quantity, actualItem.Quantity);
                Assert.AreEqual(item.Price, actualItem.Price);
                Assert.AreEqual(item.Name, actualItem.Name);
            }
        }
    }
}