// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using AutoMapper;
using IdentityServer4.Models;

namespace IdentityServer4.Firestore.Storage.Mappers
{
    public static class IdentityResourceMappers
    {
        static IdentityResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<IdentityResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static IdentityResource ToModel(this Entities.IdentityResource entity)
        {
            return entity == null ? null : Mapper.Map<IdentityResource>(entity);
        }

        public static IEnumerable<IdentityResource> ToModel(this IEnumerable<Entities.IdentityResource> entities)
        {
            return entities == null ? null : Mapper.Map<IEnumerable<IdentityResource>>(entities);
        }

        public static Entities.IdentityResource ToEntity(this IdentityResource model)
        {
            return model == null ? null : Mapper.Map<Entities.IdentityResource>(model);
        }
    }
}