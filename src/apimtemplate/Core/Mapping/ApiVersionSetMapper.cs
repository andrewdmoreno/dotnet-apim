﻿using Apim.DevOps.Toolkit.ApimEntities.ApiVersionSet;
using Apim.DevOps.Toolkit.Core.DeploymentDefinitions.Entities;
using AutoMapper;

namespace Apim.DevOps.Toolkit.Core.Mapping
{
	public static class ApiVersionSetMapper
	{
		internal static void Map(IMapperConfigurationExpression cfg)
		{
			cfg.CreateMap<ApiVersionSetDeploymentDefinition, ApiVersionSetProperties>();
		}
	}
}
