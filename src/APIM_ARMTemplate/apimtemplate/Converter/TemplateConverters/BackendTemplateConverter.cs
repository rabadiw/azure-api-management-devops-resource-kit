using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Convert
{
    public class BackendTemplateConverter
    {
        public BackendTemplateConverter(string backendTemplateFile)
        {
            BackendTemplateFile = backendTemplateFile;
        }

        public string BackendTemplateFile { get; }

        public Task<Create.CreatorConfig> ConvertAsync()
        {
            var backendTemplate = ConverterExtensions.DeserializeBackendTemplate(BackendTemplateFile);

            var creatorConfig = new Create.CreatorConfig();

            foreach (var beResource in backendTemplate.Resources)
            {
                creatorConfig.backends.Add(new Common.BackendTemplateProperties()
                {
                    title = beResource.Name,
                    url = beResource.Properties.Url,
                    //protocol = beResource.
                });
            }

            return Task.FromResult(creatorConfig);
        }
    }

    public static class ConverterExtensions
    {
        public static BackendTemplate DeserializeBackendTemplate(string templatePath)
        {
            using StreamReader r = new StreamReader(templatePath);
            string backendJson = r.ReadToEnd();
            return JsonConvert.DeserializeObject<BackendTemplate>(backendJson);
        }

        public static string[] GetBackendsTemplateJson(string templatePath) =>
            Directory.GetFiles(templatePath, "*-backends.template.json");
    }
}
