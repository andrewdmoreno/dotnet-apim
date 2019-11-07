﻿using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apim.DevOps.Toolkit.ApimEntities.Tag
{
	public class TagTemplateResouce : TemplateResource<TagPropertise>
	{
		public override string Type => ResourceType.Tag;
	}
}
