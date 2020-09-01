using System.Dynamic;
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

        public Task<Create.CreatorConfig> ConvertAsync(Create.CreatorConfig creatorConfig = null)
        {
            var backendTemplate = ConverterExtensions.DeserializeBackendTemplate(BackendTemplateFile);

            creatorConfig ??= new Create.CreatorConfig()
            {
                backends = new System.Collections.Generic.List<Common.BackendTemplateProperties>()
            };

            foreach (var beResource in backendTemplate.resources)
            {
                creatorConfig.backends.Add(new Common.BackendTemplateProperties()
                {
                    title = beResource.properties.title,
                    description = beResource?.properties?.description,
                    url = beResource?.properties?.url,
                    protocol = beResource?.properties?.protocol,
                    credentials = new Common.BackendCredentials()
                    {
                        // authorization = null,
                        // certificate = null,
                        header = beResource?.properties?.credentials?.header,
                        // query = null

                    },
                    //proxy = null,
                    //tls = null,
                    //properties = null,
                    //resourceId = beResource?.Properties?.ResourceId
                });
            }

            return Task.FromResult(creatorConfig);
        }
    }

    public static class ConverterExtensions
    {
        public static dynamic DeserializeBackendTemplate(string templatePath)
        {
            using StreamReader r = new StreamReader(templatePath);
            string backendJson = r.ReadToEnd();
            return JsonConvert.DeserializeObject<ExpandoObject>(backendJson);
        }

        public static string[] GetBackendsTemplateJson(string templatePath) =>
            Directory.GetFiles(templatePath, "*-backends.template.json");
    }
}
