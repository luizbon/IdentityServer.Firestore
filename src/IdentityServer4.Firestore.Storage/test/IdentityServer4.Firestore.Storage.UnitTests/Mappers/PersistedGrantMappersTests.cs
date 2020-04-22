using IdentityServer4.Firestore.Storage.Mappers;
using IdentityServer4.Models;
using Xunit;

namespace IdentityServer4.Firestore.Storage.UnitTests.Mappers
{
    public class PersistedGrantMappersTests
    {
        [Fact]
        public void CanMap()
        {
            PersistedGrant model = new PersistedGrant();
            Entities.PersistedGrant mappedEntity = model.ToEntity();
            PersistedGrant mappedModel = mappedEntity.ToModel();

            Assert.NotNull(mappedModel);
            Assert.NotNull(mappedEntity);
        }

        [Fact]
        public void PersistedGrantAutomapperConfigurationIsValid()
        {
            PersistedGrantMappers.Mapper.ConfigurationProvider
                .AssertConfigurationIsValid<PersistedGrantMapperProfile>();
        }
    }
}