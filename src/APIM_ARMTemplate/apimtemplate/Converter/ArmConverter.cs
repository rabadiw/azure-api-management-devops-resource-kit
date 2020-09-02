using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Convert;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Converter
{
    public class ArmConverter
    {
        private readonly string _armTemplateDirectory; 
        public ArmConverter(string armTemplateDirectory)
        {
            this._armTemplateDirectory = armTemplateDirectory;
        }

        public async Task<string> ConvertAsync()
        {
            var backendPaths = ConverterExtensions.GetBackendsTemplateJson(_armTemplateDirectory);
            
            var backendJsonFile = backendPaths.First();
            var backendCreatorConfigMapper = new BackendCreatorConfigMapper(backendJsonFile);

            var creatorConfig = await backendCreatorConfigMapper.ConvertAsync();
            
            var yamlSerializer = new YamlDotNet.Serialization.SerializerBuilder().Build();
            
            return yamlSerializer.Serialize(creatorConfig.backends[0]);
        }
    }
}