﻿using System.Threading.Tasks;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Extract
{
    public class BackendExtractor : EntityExtractor
    {
        public async Task<string> GetBackendsAsync(string ApiManagementName, string ResourceGroupName)
        {
            (string azToken, string azSubId) = await auth.GetAccessToken();

            string requestUrl = string.Format("{0}/subscriptions/{1}/resourceGroups/{2}/providers/Microsoft.ApiManagement/service/{3}/backends?api-version={4}",
               baseUrl, azSubId, ResourceGroupName, ApiManagementName, GlobalConstants.ApiVersion);

            return await CallApiManagementAsync(azToken, requestUrl);
        }

        public async Task<string> GetBackendDetailsAsync(string ApiManagementName, string ResourceGroupName, string backendName)
        {
            (string azToken, string azSubId) = await auth.GetAccessToken();

            string requestUrl = string.Format("{0}/subscriptions/{1}/resourceGroups/{2}/providers/Microsoft.ApiManagement/service/{3}/backends/{4}?api-version={5}",
               baseUrl, azSubId, ResourceGroupName, ApiManagementName, backendName, GlobalConstants.ApiVersion);

            return await CallApiManagementAsync(azToken, requestUrl);
        }

        public async Task<Template> GenerateBackendsARMTemplateAsync(string apimname, string resourceGroup, string singleApiName, List<TemplateResource> apiTemplateResources, List<TemplateResource> propertyResources, string policyXMLBaseUrl)
        {
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("Extracting backends from service");
            Template armTemplate = GenerateEmptyTemplateWithParameters(policyXMLBaseUrl);

            List<TemplateResource> templateResources = new List<TemplateResource>();

            // isolate api and operation policy resources in the case of a single api extraction, as they may reference backends
            var policyResources = apiTemplateResources.Where(resource => (resource.Type == ResourceType.ApiPolicy || resource.Type == ResourceType.ApiOperationPolicy || resource.Type == ResourceType.ProductPolicy));
            var namedValueResources = propertyResources.Where(resource => (resource.Type == ResourceType.Property));

            // pull all backends for service
            string backends = await GetBackendsAsync(apimname, resourceGroup);
            JObject oBackends = JObject.Parse(backends);

            foreach (var item in oBackends["value"])
            {
                string backendName = ((JValue)item["name"]).Value.ToString();
                string backend = await GetBackendDetailsAsync(apimname, resourceGroup, backendName);

                // convert returned backend to template resource class
                BackendTemplateResource backendTemplateResource = JsonConvert.DeserializeObject<BackendTemplateResource>(backend);
                backendTemplateResource.Name = $"[concat(parameters('ApimServiceName'), '/{backendName}')]";
                backendTemplateResource.ApiVersion = GlobalConstants.ApiVersion;

                ////only extract the backend if this is a full extraction, or in the case of a single api, if it is referenced by one of the policies
                //if (singleApiName == null)
                //{
                //    // if the user is extracting all apis, extract all the backends
                //    Console.WriteLine("'{0}' Backend found", backendName);
                //    templateResources.Add(backendTemplateResource);
                //}
                //else
                //{
                //    bool isReferencedInPolicy = false;
                //    foreach (PolicyTemplateResource policyTemplateResource in policyResources)
                //    {
                //        // the backend is used in a policy if the xml contains a set-backend-service policy, which will reference the backend's url or id
                //        string policyContent = policyTemplateResource.Properties.policyContent;
                //        isReferencedInPolicy = DoesPolicyReferenceBackend(policyContent, namedValueResources,  backendName, backendTemplateResource);
                //    }
                //    if (isReferencedInPolicy == true)
                //    {
                //        // backend was used in policy, extract it
                //        Console.WriteLine("'{0}' Backend found", backendName);
                //        templateResources.Add(backendTemplateResource);
                //    }
                //}

                Console.WriteLine("'{0}' Backend found", backendName);
                templateResources.Add(backendTemplateResource);
            }

            armTemplate.resources = templateResources.ToArray();
            return armTemplate;
        }

        public bool DoesPolicyReferenceBackend(string policyContent, IEnumerable<TemplateResource> namedValueResources, string backendName, BackendTemplateResource backendTemplateResource)
        {
            // a policy is referenced by a backend with the set-backend-service policy, which will reference use the backends name or url, or through referencing a named value that applies to the backend
            var namedValueResourcesUsedByBackend = namedValueResources.Where(resource => DoesBackendReferenceNamedValue(resource, backendTemplateResource));
            if ((backendName != null && policyContent.Contains(backendName)) || 
                (backendTemplateResource.Properties.Url != null && policyContent.Contains(backendTemplateResource.Properties.Url)) || 
                (backendTemplateResource.Properties.Title != null && policyContent.Contains(backendTemplateResource.Properties.Title)) ||
                (backendTemplateResource.Properties.ResourceId != null && policyContent.Contains(backendTemplateResource.Properties.ResourceId)))
            {
                return true;
            }
            foreach(PropertyTemplateResource namedValueResource in namedValueResourcesUsedByBackend)
            {
                if (policyContent.Contains(namedValueResource.Properties.DisplayName) || policyContent.Contains(namedValueResource.Properties.Value))
                {
                    return true;
                }

            }
            return false;
        }

        public bool DoesBackendReferenceNamedValue(TemplateResource namedValueResource, BackendTemplateResource backendTemplateResource)
        {
            string namedValue = (namedValueResource as PropertyTemplateResource).Properties.Value;
            return (namedValue == backendTemplateResource.Properties.Url 
                || namedValue == backendTemplateResource.Properties.Description
                || namedValue == backendTemplateResource.Properties.Title);
        }
    }
}
