﻿using Apim.DevOps.Toolkit.ArmTemplates;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Create
{
	public class CertificateDeploymentDefinition : CertificateProperties
	{
		/// <summary>
		/// The Id of the Certificate
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The path to the pfx certificate
		/// </summary>
		public string FilePath { get; set; }
	}

}