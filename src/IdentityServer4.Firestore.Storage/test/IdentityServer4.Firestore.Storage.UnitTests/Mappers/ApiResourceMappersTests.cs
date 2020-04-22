using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using IdentityServer4.Firestore.Storage.Mappers;
using IdentityServer4.Models;
using Xunit;
using Secret = IdentityServer4.Firestore.Storage.Entities.Secret;

namespace IdentityServer4.Firestore.Storage.UnitTests.Mappers
{
    public class ApiResourceMappersTests
    {
        [Fact]
        public void AutomapperConfigurationIsValid()
        {
            ApiResourceMappers.Mapper.ConfigurationProvider.AssertConfigurationIsValid<ApiResourceMapperProfile>();
        }

        [Fact]
        public void Can_Map()
        {
            ApiResource model = new ApiResource();
            Entities.ApiResource mappedEntity = model.ToEntity();
            ApiResource mappedModel = mappedEntity.ToModel();

            Assert.NotNull(mappedModel);
            Assert.NotNull(mappedEntity);
        }

        [Fact]
        public void missing_values_should_use_defaults()
        {
            Entities.ApiResource entity = new Entities.ApiResource {Secrets = new List<Secret> {new Secret()}};

            ApiResource def = new ApiResource {ApiSecrets = {new Models.Secret("foo")}};

            ApiResource model = entity.ToModel();
            model.ApiSecrets.First().Type.Should().Be(def.ApiSecrets.First().Type);
        }

        [Fact]
        public void Properties_Map()
        {
            ApiResource model = new ApiResource
            {
                Description = "description",
                DisplayName = "displayname",
                Name = "foo",
                Scopes = {"foo1", "foo2"},
                Enabled = false
            };

            Entities.ApiResource mappedEntity = model.ToEntity();

            mappedEntity.Scopes.Count.Should().Be(2);
            string foo1 = mappedEntity.Scopes.FirstOrDefault(x => x == "foo1");
            foo1.Should().NotBeNull();
            string foo2 = mappedEntity.Scopes.FirstOrDefault(x => x == "foo2");
            foo2.Should().NotBeNull();


            ApiResource mappedModel = mappedEntity.ToModel();

            mappedModel.Description.Should().Be("description");
            mappedModel.DisplayName.Should().Be("displayname");
            mappedModel.Enabled.Should().BeFalse();
            mappedModel.Name.Should().Be("foo");
        }
    }
}