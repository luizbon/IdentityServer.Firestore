using FluentAssertions;
using IdentityServer4.Firestore.Storage.Mappers;
using IdentityServer4.Models;
using Xunit;

namespace IdentityServer4.Firestore.Storage.UnitTests.Mappers
{
    public class ScopesMappersTests
    {
        [Fact]
        public void CanMapScope()
        {
            ApiScope model = new ApiScope();
            Entities.ApiScope mappedEntity = model.ToEntity();
            ApiScope mappedModel = mappedEntity.ToModel();

            Assert.NotNull(mappedModel);
            Assert.NotNull(mappedEntity);
        }

        [Fact]
        public void Properties_Map()
        {
            ApiScope model = new ApiScope
            {
                Description = "description",
                DisplayName = "displayname",
                Name = "foo",
                UserClaims = {"c1", "c2"},
                Properties = {{"x", "xx"}, {"y", "yy"}},
                Enabled = false
            };


            Entities.ApiScope mappedEntity = model.ToEntity();
            mappedEntity.Description.Should().Be("description");
            mappedEntity.DisplayName.Should().Be("displayname");
            mappedEntity.Name.Should().Be("foo");

            mappedEntity.UserClaims.Count.Should().Be(2);
            mappedEntity.UserClaims.Should().BeEquivalentTo("c1", "c2");
            mappedEntity.Properties.Count.Should().Be(2);
            mappedEntity.Properties.Should().Contain("x", "xx");
            mappedEntity.Properties.Should().Contain("y", "yy");


            ApiScope mappedModel = mappedEntity.ToModel();

            mappedModel.Description.Should().Be("description");
            mappedModel.DisplayName.Should().Be("displayname");
            mappedModel.Enabled.Should().BeFalse();
            mappedModel.Name.Should().Be("foo");
            mappedModel.UserClaims.Count.Should().Be(2);
            mappedModel.UserClaims.Should().BeEquivalentTo("c1", "c2");
            mappedModel.Properties.Count.Should().Be(2);
            mappedModel.Properties["x"].Should().Be("xx");
            mappedModel.Properties["y"].Should().Be("yy");
        }

        [Fact]
        public void ScopeAutomapperConfigurationIsValid()
        {
            ScopeMappers.Mapper.ConfigurationProvider.AssertConfigurationIsValid<ScopeMapperProfile>();
        }
    }
}