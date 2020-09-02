using McMaster.Extensions.CommandLineUtils;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;
using System;
using System.IO;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Converter;

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

                    var armConverter = new ArmConverter(templatePath);
                    var result = await armConverter.ConvertAsync();
                    Console.WriteLine(result);

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
    }
}