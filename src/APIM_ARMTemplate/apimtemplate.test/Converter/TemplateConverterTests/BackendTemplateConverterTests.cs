using FluentAssertions;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Convert;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Test.Convert
{
    public class BackendTemplateConverterTests
    {
        [Fact]
        public async Task ShouldConvertBackendTemplateToCreatorConfig()
        {
            var templatePath = Path.GetFullPath("../../../../apimtemplate/Creator/ExampleGeneratedTemplates");
            var backendPaths = ConverterExtensions.GetBackendsTemplateJson(templatePath);

            var backendJsonFile = backendPaths.First();
            var converter = new BackendTemplateConverter(backendJsonFile);
            var creatorConfig = await converter.ConvertAsync();

            creatorConfig.backends.Count.Should().Be(1);
            creatorConfig.backends.First().title.Should().Be("myBackend");
            creatorConfig.backends.First().description.Should().Be("description5308");
        }
    }
}
