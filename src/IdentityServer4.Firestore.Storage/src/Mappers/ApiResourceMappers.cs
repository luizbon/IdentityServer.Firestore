// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using AutoMapper;
using IdentityServer4.Models;

namespace IdentityServer4.Firestore.Storage.Mappers
{
    public static class ApiResourceMappers
    {
        static ApiResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static ApiResource ToModel(this Entities.ApiResource entity)
        {
            return entity == null ? null : Mapper.Map<ApiResource>(entity);
        }

        public static IEnumerable<ApiResource> ToModel(this IEnumerable<Entities.ApiResource> entities)
        {
            return entities == null ? null : Mapper.Map<IEnumerable<ApiResource>>(entities);
        }

        public static Entities.ApiResource ToEntity(this ApiResource model)
        {
            return model == null ? null : Mapper.Map<Entities.ApiResource>(model);
        }
    }
}