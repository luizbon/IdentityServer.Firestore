// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AutoMapper;
using IdentityServer4.Firestore.Storage.Entities;

namespace IdentityServer4.Firestore.Storage.Mappers
{
    public class ApiResourceMapperProfile : Profile
    {
        public ApiResourceMapperProfile()
        {
            CreateMap<ApiResource, Models.ApiResource>(MemberList.Destination)
                .ConstructUsing(src => new Models.ApiResource())
                .ForMember(x => x.ApiSecrets, opts => opts.MapFrom(x => x.Secrets))
                .ForMember(x => x.AllowedAccessTokenSigningAlgorithms,
                    opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter,
                        x => x.AllowedAccessTokenSigningAlgorithms))
                .ReverseMap()
                .ForMember(x => x.AllowedAccessTokenSigningAlgorithms,
                    opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter,
                        x => x.AllowedAccessTokenSigningAlgorithms));

            CreateMap<Secret, Models.Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();
        }
    }
}