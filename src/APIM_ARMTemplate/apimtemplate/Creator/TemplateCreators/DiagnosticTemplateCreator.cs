﻿using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Create
{
    public class DiagnosticTemplateCreator
    {
        public DiagnosticTemplateResource CreateAPIDiagnosticTemplateResource(ApiConfiguration api, string[] dependsOn)
        {
            // create diagnostic resource with properties
            DiagnosticTemplateResource diagnosticTemplateResource = new DiagnosticTemplateResource()
            {
                name = $"[concat(parameters('ApimServiceName'), '/{api.name}/{api.diagnostic.name}')]",
                type = ResourceType.ApiDiagnostic,
                apiVersion = GlobalConstants.APIVersion,
                properties = new DiagnosticTemplateProperties()
                {
                    alwaysLog = api.diagnostic.alwaysLog,
                    sampling = api.diagnostic.sampling,
                    frontend = api.diagnostic.frontend,
                    backend = api.diagnostic.backend,
                    enableHttpCorrelationHeaders = api.diagnostic.enableHttpCorrelationHeaders
                },
                dependsOn = dependsOn
            };
            // reference the provided logger if loggerId is provided
            if (api.diagnostic.loggerId != null)
            {
                diagnosticTemplateResource.properties.loggerId = $"[resourceId('Microsoft.ApiManagement/service/loggers', parameters('ApimServiceName'), '{api.diagnostic.loggerId}')]";
            }
            return diagnosticTemplateResource;
        }
    }
}
