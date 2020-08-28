using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using apimtemplate.Creator.Utilities;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Create;
using Newtonsoft.Json;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Convert
{
    public class ConvertCommand : CommandLineApplication
    {
        public ConvertCommand()
        {
            this.Name = GlobalConstants.ConvertName;
            this.Description = GlobalConstants.ConvertDescription;
            var converterTempaltePathOption = this.Option(
                "--templatePath <templatePath>",
                "Path to the extracted templates",
                CommandOptionType.SingleValue);

            this.HelpOption();

            this.OnExecute(async () =>
            {
                string templatePath = default;
                if (converterTempaltePathOption.HasValue())
                {
                    templatePath = converterTempaltePathOption.Value();
                }

                try
                {
                    // Guards

                    if (!Directory.Exists(templatePath))
                    {
                        throw new ArgumentException("Directory not found.", nameof(templatePath));
                    }

                    if (Directory.GetFiles(templatePath).Length == 0)
                    {
                        throw new ArgumentException("Directory is empty.", nameof(templatePath));
                    }

                    // For now work on the backends
                    foreach (var beJsonFile in Directory.GetFiles(templatePath, "*-backends.tempalte.json"))
                    {
                        var backendTemplate = DeserializeBackendTemplate(beJsonFile);

                        var creatorConfig = new CreatorConfig();

                        foreach (var beResource in backendTemplate.Resources)
                        {
                            creatorConfig.backends.Add(new BackendTemplateProperties()
                            {
                                title = beResource.Name,
                                url = beResource.Properties.Url,
                                protocol = beResource.
                            });
                        }

                    }



                    Console.WriteLine("Press any key to exit process:");
#if DEBUG
                    Console.ReadKey();
#endif
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message);
                    throw;
                }
            });
        }

        private Models.BackendTemplate DeserializeBackendTemplate(string templatePath)
        {
            using (StreamReader r = new StreamReader(templatePath))
            {
                string backendJson = r.ReadToEnd();
                Models.BackendTemplate backendTemplate = JsonConvert.DeserializeObject<Models.BackendTemplate>(backendJson);
                return backendTemplate;
            }
        }

    }
}