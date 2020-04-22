// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Security.Claims;
using AutoMapper;
using IdentityServer4.Firestore.Storage.Entities;

namespace IdentityServer4.Firestore.Storage.Mappers
{
    /// <summary>
    ///     Defines entity/model mapping for clients.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class ClientMapperProfile : Profile
    {
        /// <summary>
        ///     <see>
        ///         <cref>{ClientMapperProfile}</cref>
        ///     </see>
        /// </summary>
        public ClientMapperProfile()
        {
            CreateMap<Client, Models.Client>(MemberList.Destination)
                .ForMember(dest => dest.ProtocolType, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.AllowedIdentityTokenSigningAlgorithms,
                    opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter,
                        x => x.AllowedIdentityTokenSigningAlgorithms))
                .ReverseMap()
                .ForMember(x => x.AllowedIdentityTokenSigningAlgorithms,
                    opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter,
                        x => x.AllowedIdentityTokenSigningAlgorithms));

            CreateMap<ClientClaim, Models.ClientClaim>(MemberList.None)
                .ConstructUsing(src => new Models.ClientClaim(src.Type, src.Value, ClaimValueTypes.String))
                .ReverseMap();

            CreateMap<Secret, Models.Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();
        }
    }
}