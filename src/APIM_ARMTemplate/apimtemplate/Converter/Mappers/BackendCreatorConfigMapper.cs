using System.Collections.Generic;
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
                var root = beResource as IDictionary<string, object>;
                var properties = (root?.ContainsKey("properties")).GetValueOrDefault()
                    ? beResource?.properties as IDictionary<string, object>
                    : null;
                var credentials = (properties?.ContainsKey("credentials")).GetValueOrDefault()
                    ? beResource?.properties?.credentials as IDictionary<string, object>
                    : null;
                var proxy = (properties?.ContainsKey("proxy")).GetValueOrDefault()
                    ? beResource?.properties?.proxy as IDictionary<string, object>
                    : null;
                var tls = (properties?.ContainsKey("tls")).GetValueOrDefault()
                    ? beResource?.properties?.tls as IDictionary<string, object>
                    : null;

                creatorConfig.backends.Add(new Common.BackendTemplateProperties()
                {
                    title = properties?.GetValueOrDefault<string>("title"),
                    description = properties?.GetValueOrDefault<string>("description"),
                    url = properties?.GetValueOrDefault<string>("url"),
                    protocol = properties?.GetValueOrDefault<string>("protocol"),
                    credentials = new Common.BackendCredentials()
                    {
                        authorization = credentials?.GetValueOrDefault<object>("authorization"),
                        header = credentials?.GetValueOrDefault<object>("header"),
                        query = credentials?.GetValueOrDefault<object>("query"),
                    },
                    proxy = proxy == null
                        ? null
                        : new BackendProxy
                        {
                            url = proxy?.GetValueOrDefault<string>("url"),
                            username = proxy?.GetValueOrDefault<string>("username"),
                            password = proxy?.GetValueOrDefault<string>("password"),
                        },

                    tls = tls == null
                        ? null
                        : new BackendTLS
                        {
                            validateCertificateChain = (tls?.GetValueOrDefault<bool>("validateCertificateChain"))
                                .GetValueOrDefault(),
                            validateCertificateName = (tls?.GetValueOrDefault<bool>("validateCertificateName"))
                                .GetValueOrDefault(),
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

        public static T GetValueOrDefault<T>(this IDictionary<string, object> collection, string key)
        {
            if (collection.TryGetValue(key, out var value))
            {
                return (T) value;
            }

            return default;
        }
    }
}