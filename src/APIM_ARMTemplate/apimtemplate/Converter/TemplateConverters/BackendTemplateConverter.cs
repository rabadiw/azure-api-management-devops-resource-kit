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

            foreach (var beResource in backendTemplate.Resources)
            {
                creatorConfig.backends.Add(new Common.BackendTemplateProperties()
                {
                    title = beResource.Properties.Title,
                    description = beResource?.Properties?.Description,
                    url = beResource?.Properties?.Url,
                    protocol = beResource?.Properties?.Protocol,
                    credentials = new Common.BackendCredentials()
                    {
                        // authorization = null,
                        // certificate = null,
                        header = beResource?.Properties?.Credentials?.Header,
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
