using System.Dynamic;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Convert
{
    public class BackendCreatorConfigMapper
    {
        
        public BackendCreatorConfigMapper(string backendTemplateFile)
        {
            _backendTemplateFile = backendTemplateFile;
        }

        private readonly string _backendTemplateFile; 

        public Task<Create.CreatorConfig> ConvertAsync(Create.CreatorConfig creatorConfig = null)
        {
            var backendTemplate = ConverterExtensions.DeserializeBackendTemplate(_backendTemplateFile);
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
                        authorization = beResource?.properties?.credentials?.authorization,
                        header = beResource?.properties?.credentials?.header,
                        query = beResource?.properties?.credentials?.query,
                    },
                    proxy = new BackendProxy
                    {
                        url = beResource?.properties?.proxy?.url,
                        username = beResource?.properties?.proxy?.username,
                        password = beResource?.properties?.proxy?.password,
                    },
                        
                    tls = new BackendTLS
                    {
                        validateCertificateChain = beResource?.properties?.tls?.validateCertificateChain,
                        validateCertificateName = beResource?.properties?.tls?.validateCertificateName,
                    }
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
