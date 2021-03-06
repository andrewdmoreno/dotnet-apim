﻿using Apim.DevOps.Toolkit.ApimEntities.Tag;
using Apim.DevOps.Toolkit.Core.DeploymentDefinitions.Entities;
using AutoMapper;

namespace Apim.DevOps.Toolkit.Core.Mapping
{
	public static class TagMapper
	{
		internal static void Map(IMapperConfigurationExpression cfg)
		{
			cfg.CreateMap<TagDeploymentDefinition, TagProperties>();
		}
	}
}
