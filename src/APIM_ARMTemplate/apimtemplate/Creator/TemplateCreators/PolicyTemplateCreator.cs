﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apim.Arm.Creator.Creator.TemplateCreators;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Create
{
    public class PolicyTemplateCreator: TemplateCreator,ITemplateCreator
    {
        private FileReader _fileReader;

        public PolicyTemplateCreator()
        {
            _fileReader = new FileReader();
        }

        public async Task<Template> Create(CreatorConfig creatorConfig)
        {
            var template = EmptyTemplate;
            template.Parameters.Add(ApiServiceNameParameter.Key, ApiServiceNameParameter.Value);

            template.resources = new TemplateResource[1]
            {
                CreateOperationPolicyTemplateResource(ResourceType.GlobalServicePolicy, creatorConfig.Policy, $"policy", new string[0])
            }; 

            return await Task.FromResult(template);
        }

        public PolicyTemplateResource CreateAPIPolicyTemplateResource(ApiConfiguration api, string[] dependsOn)
        {
            return CreateOperationPolicyTemplateResource(ResourceType.ApiPolicy, api.policy, $"{api.name}/policy", dependsOn);  
        }

        public PolicyTemplateResource CreateOperationPolicyTemplateResource(string policyType ,string policy, string name, string[] dependsOn)
        {
            bool isUrl = Uri.TryCreate(policy, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            
            PolicyTemplateResource policyTemplateResource = new PolicyTemplateResource(policyType)
            {
                Name = $"[concat(parameters('ApimServiceName'), '/{name}')]",
                Properties = new PolicyTemplateProperties()
                {
                    // if policy is a url inline the url, if it is a local file inline the file contents
                    Format = isUrl ? "rawxml-link" : "rawxml",
                    Value = isUrl ? policy : this._fileReader.RetrieveLocalFileContents(policy)
                },
                DependsOn = dependsOn
            };
            return policyTemplateResource;
        }

        public List<PolicyTemplateResource> CreateOperationPolicyTemplateResources(ApiConfiguration api, string[] dependsOn)
        {
            // create a policy resource for each policy listed in the config file and its associated provided xml file
            List<PolicyTemplateResource> policyTemplateResources = new List<PolicyTemplateResource>();
            foreach (KeyValuePair<string, OperationsConfig> pair in api.operations)
            {
                policyTemplateResources.Add(CreateOperationPolicyTemplateResource(ResourceType.ApiOperationPolicy, pair.Value.Policy, $"{api.name}/{pair.Key}/policy", dependsOn));
            }
            return policyTemplateResources;
        }

    }
}
