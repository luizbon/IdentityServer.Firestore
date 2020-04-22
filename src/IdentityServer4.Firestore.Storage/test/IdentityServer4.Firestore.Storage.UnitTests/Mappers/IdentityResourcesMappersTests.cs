using IdentityServer4.Firestore.Storage.Mappers;
using IdentityServer4.Models;
using Xunit;

namespace IdentityServer4.Firestore.Storage.UnitTests.Mappers
{
    public class IdentityResourcesMappersTests
    {
        [Fact]
        public void CanMapIdentityResources()
        {
            IdentityResource model = new IdentityResource();
            Entities.IdentityResource mappedEntity = model.ToEntity();
            IdentityResource mappedModel = mappedEntity.ToModel();

            Assert.NotNull(mappedModel);
            Assert.NotNull(mappedEntity);
        }

        [Fact]
        public void IdentityResourceAutomapperConfigurationIsValid()
        {
            IdentityResourceMappers.Mapper.ConfigurationProvider
                .AssertConfigurationIsValid<IdentityResourceMapperProfile>();
        }
    }
}