﻿using Apim.DevOps.Toolkit.ApimEntities.Logger;
using Apim.DevOps.Toolkit.Core.DeploymentDefinitions.Entities;
using AutoMapper;

namespace Apim.DevOps.Toolkit.Core.Mapping
{
	public static class LoggerMapper
	{
		internal static void Map(IMapperConfigurationExpression cfg)
		{
			cfg.CreateMap<LoggerDeploymentDefinition, LoggerProperties>();
		}
	}
}
