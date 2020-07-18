using Apim.DevOps.Toolkit.Core.Configuration;
using Apim.DevOps.Toolkit.Core.Infrastructure;
using Apim.DevOps.Toolkit.Core.Templates;
using Apim.DevOps.Toolkit.Core.Variables;
using System;
using System.Threading.Tasks;

namespace Apim.DevOps.Toolkit.CommandLine.Commands
{
	public class CreateCommand
	{
		private static readonly FileReader _fileReader = new FileReader();

		public async Task Process(CommandLineOption option)
		{
			await LoadGlobalVariables(option);

			var deploymentDefinition = await GetDeploymentDefinition(option);

			await CreateTemplates(deploymentDefinition);
		}

		private async Task CreateTemplates(DeploymentDefinition deploymentDefinition)
		{
			var armTemplateCreator = new ArmTemplateCreator(deploymentDefinition, null);
			await armTemplateCreator.Create();
		}

		private async Task<DeploymentDefinition> GetDeploymentDefinition(CommandLineOption option)
		{
			var deploymentDefinition = await _fileReader.GetDeploymentDefinitionFromYaml(option.YamlConfigPath);

			deploymentDefinition.PrefixFileName = option.FileNamePrefix;
			deploymentDefinition.MasterTemplateName = option.MasterFileName;

			foreach (var productDeploymentDefinition in deploymentDefinition.Products)
			{
				productDeploymentDefinition.Root = deploymentDefinition;
			}

			foreach (var apiDeploymentDefinition in deploymentDefinition.Apis)
			{
				apiDeploymentDefinition.Root = deploymentDefinition;
			}

			// TODO: Fix hierarchy

			return deploymentDefinition;
		}

		private async Task LoadGlobalVariables(CommandLineOption option)
		{
			await VariableReplacer.Instance.LoadFromFile(option.VariableFilePath);
			VariableReplacer.Instance.LoadFromString(option.VariableString);

			if (option.PrintVariables)
			{
				foreach (var variable in VariableReplacer.Instance.Variables)
				{
					Console.WriteLine($"variable is loaded: {variable.Key}={variable.Value}");
				}
			}
		}
	}
}
