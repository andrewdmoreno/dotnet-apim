﻿using Xunit;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Create;
using System.Collections.Generic;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Test
{
    public class LoggerTemplateCreatorTests
    {
        [Fact]
        public void ShouldCreateLoggerTemplateFromCreatorConfig()
        {
            // arrange
            LoggerTemplateCreator loggerTemplateCreator = new LoggerTemplateCreator();
            DeploymentDefinition creatorConfig = new DeploymentDefinition() { Loggers = new List<LoggerDeploymentDefinition>() };
            LoggerDeploymentDefinition logger = new LoggerDeploymentDefinition()
            {
                Name = "name",
                LoggerType = "applicationinsights",
                Description = "description",
                IsBuffered = true,
                ResourceId = "resourceId",
                Credentials = new LoggerCredentials()
                {
                    ConnectionString = "connString",
                    InstrumentationKey = "iKey",
                    Name = "credName"
                }

            };
            creatorConfig.Loggers.Add(logger);

            // act
            Template loggerTemplate = loggerTemplateCreator.CreateLoggerTemplate(creatorConfig);
            LoggerTemplateResource loggerTemplateResource = (LoggerTemplateResource)loggerTemplate.Resources[0];

            // assert
            Assert.Equal($"[concat(parameters('ApimServiceName'), '/{logger.Name}')]", loggerTemplateResource.Name);
            Assert.Equal(logger.LoggerType, loggerTemplateResource.properties.LoggerType);
            Assert.Equal(logger.Description, loggerTemplateResource.properties.Description);
            Assert.Equal(logger.IsBuffered, loggerTemplateResource.properties.IsBuffered);
            Assert.Equal(logger.ResourceId, loggerTemplateResource.properties.ResourceId);
            Assert.Equal(logger.Credentials.ConnectionString, loggerTemplateResource.properties.Credentials.connectionString);
            Assert.Equal(logger.Credentials.InstrumentationKey, loggerTemplateResource.properties.Credentials.instrumentationKey);
            Assert.Equal(logger.Credentials.Name, loggerTemplateResource.properties.Credentials.name);
        }
    }
}
