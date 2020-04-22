// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using AutoMapper;
using IdentityServer4.Models;

namespace IdentityServer4.Firestore.Storage.Mappers
{
    public static class ScopeMappers
    {
        static ScopeMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ScopeMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static ApiScope ToModel(this Entities.ApiScope entity)
        {
            return entity == null ? null : Mapper.Map<ApiScope>(entity);
        }

        public static IEnumerable<ApiScope> ToModel(this IEnumerable<Entities.ApiScope> entities)
        {
            return entities == null ? null : Mapper.Map<IEnumerable<ApiScope>>(entities);
        }

        public static Entities.ApiScope ToEntity(this ApiScope model)
        {
            return model == null ? null : Mapper.Map<Entities.ApiScope>(model);
        }
    }
}