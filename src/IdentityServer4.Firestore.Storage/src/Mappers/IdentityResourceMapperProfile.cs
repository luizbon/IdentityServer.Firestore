﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AutoMapper;
using IdentityServer4.Firestore.Storage.Entities;

namespace IdentityServer4.Firestore.Storage.Mappers
{
    /// <summary>
    ///     Defines entity/model mapping for identity resources.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class IdentityResourceMapperProfile : Profile
    {
        /// <summary>
        ///     <see cref="IdentityResourceMapperProfile" />
        /// </summary>
        public IdentityResourceMapperProfile()
        {
            CreateMap<IdentityResource, Models.IdentityResource>(MemberList.Destination)
                .ConstructUsing(src => new Models.IdentityResource())
                .ReverseMap();
        }
    }
}