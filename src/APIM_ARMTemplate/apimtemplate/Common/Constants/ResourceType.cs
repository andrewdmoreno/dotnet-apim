﻿
namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common
{
    internal static class ResourceType
    {
        public const string Api = "Microsoft.ApiManagement/service/apis";
        public const string ApiVersionSet = "Microsoft.ApiManagement/service/apiVersionSets";
        public const string ApiDiagnostic = "Microsoft.ApiManagement/service/apis/diagnostics";
        public const string ApiOperation = "Microsoft.ApiManagement/service/apis/operations";
        public const string ApiOperationPolicy = "Microsoft.ApiManagement/service/apis/operations/policies";
        public const string ApiPolicy = "Microsoft.ApiManagement/service/apis/policies";
        public const string ApiRelease = "Microsoft.ApiManagement/service/apis/releases";
        public const string ApiSchema = "Microsoft.ApiManagement/service/apis/schemas";
        public const string AuthorizationServer = "Microsoft.ApiManagement/service/authorizationServers";
        public const string Backend = "Microsoft.ApiManagement/service/backends";
        public const string GlobalServicePolicy = "Microsoft.ApiManagement/service/policies";
        public const string Logger = "Microsoft.ApiManagement/service/loggers";
        public const string ProductAPI = "Microsoft.ApiManagement/service/products/apis";
        public const string Product = "Microsoft.ApiManagement/service/products";
        public const string ProductPolicy = "Microsoft.ApiManagement/service/products/policies";
        public const string Property = "Microsoft.ApiManagement/service/properties";
    }
}