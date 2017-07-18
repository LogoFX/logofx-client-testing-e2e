using System;
using System.Linq;
using Attest.Fake.Builders;
using Attest.Fake.Core;
using Attest.Fake.Moq;
using FluentAssertions;
using LogoFX.Client.Testing.EndToEnd.FakeData.Shared;
using Solid.Practices.Composition;
using Xunit;

namespace LogoFX.Client.Testing.EndToEnd.Tests
{    
    public class SerializationTests
    {
        [Fact]
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
            ConstraintFactoryContext.Current = new ConstraintFactory();
            PlatformProvider.Current = new NetPlatformProvider();
            var simpleBuilder = SimpleProviderBuilder.CreateBuilder();
            simpleBuilder.WithWarehouseItems(items);

            BuildersCollectionContext.AddBuilder(simpleBuilder);
            BuildersCollectionContext.SerializeBuilders();
            BuildersCollectionContext.DeserializeBuilders();
            var builders = BuildersCollectionContext.GetBuilders<ISimpleProvider>();
            IBuilder<ISimpleProvider> actualBuilder = builders.First();

            var actualItems = actualBuilder.Build().GetSimpleItems().ToArray();
            for (int i = 0; i < Math.Max(items.Length, actualItems.Length); i++)
            {
                var item = items[i];
                var actualItem = actualItems[i];
                actualItem.Quantity.Should().Be(item.Quantity);
                actualItem.Price.Should().Be(item.Price);
                actualItem.Name.Should().Be(item.Name);                
            }
        }
    }
}
