using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using IdentityServer4.Firestore.Storage.Mappers;
using IdentityServer4.Models;
using Xunit;
using Secret = IdentityServer4.Firestore.Storage.Entities.Secret;

namespace IdentityServer4.Firestore.Storage.UnitTests.Mappers
{
    public class ClientMappersTests
    {
        [Fact]
        public void AutomapperConfigurationIsValid()
        {
            ClientMappers.Mapper.ConfigurationProvider.AssertConfigurationIsValid<ClientMapperProfile>();
        }

        [Fact]
        public void Can_Map()
        {
            Client model = new Client();
            Entities.Client mappedEntity = model.ToEntity();
            Client mappedModel = mappedEntity.ToModel();

            Assert.NotNull(mappedModel);
            Assert.NotNull(mappedEntity);
        }

        [Fact]
        public void missing_values_should_use_defaults()
        {
            Entities.Client entity = new Entities.Client {ClientSecrets = new List<Secret> {new Secret()}};

            Client def = new Client {ClientSecrets = {new Models.Secret("foo")}};

            Client model = entity.ToModel();
            model.ProtocolType.Should().Be(def.ProtocolType);
            model.ClientSecrets.First().Type.Should().Be(def.ClientSecrets.First().Type);
        }

        [Fact]
        public void Properties_Map()
        {
            Client model = new Client {Properties = {{"foo1", "bar1"}, {"foo2", "bar2"}}};


            Entities.Client mappedEntity = model.ToEntity();

            mappedEntity.Properties.Count.Should().Be(2);
            KeyValuePair<string, string> foo1 = mappedEntity.Properties.FirstOrDefault(x => x.Key == "foo1");
            foo1.Should().NotBeNull();
            foo1.Value.Should().Be("bar1");
            KeyValuePair<string, string> foo2 = mappedEntity.Properties.FirstOrDefault(x => x.Key == "foo2");
            foo2.Should().NotBeNull();
            foo2.Value.Should().Be("bar2");


            Client mappedModel = mappedEntity.ToModel();

            mappedModel.Properties.Count.Should().Be(2);
            mappedModel.Properties.ContainsKey("foo1").Should().BeTrue();
            mappedModel.Properties.ContainsKey("foo2").Should().BeTrue();
            mappedModel.Properties["foo1"].Should().Be("bar1");
            mappedModel.Properties["foo2"].Should().Be("bar2");
        }
    }
}