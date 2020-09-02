using System.Collections.Generic;
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

            var backend = creatorConfig.backends.First();
            backend.title.Should().Be("myBackend");
            backend.description.Should().Be("description5308");
            backend.url.Should().Be("https://backendname2644/");
            backend.protocol.Should().Be("http");
            backend.credentials.Should().NotBeNull();
            var headers = ((IDictionary<string, object>) backend.credentials.header)["x-my-1"] as List<object>;
            headers.Should().Contain("val1");
            headers.Should().Contain("val2");

            var sv = backend.credentials.query.sv as List<object>;
            sv.Should().Contain("xx");
            sv.Should().Contain("bb");

            (backend.credentials.authorization.parameter as string).Should().Be("opensesma");
            (backend.credentials.authorization.scheme as string).Should().Be("Basic");
            backend.proxy.url.Should().Be("http://192.168.1.1:8080");
            backend.proxy.url.Should().Be("http://192.168.1.1:8080");
            backend.proxy.username.Should().Be("Contoso\\admin");
            backend.proxy.password.Should().Be("opensesame");
            backend.tls.validateCertificateChain.Should().BeTrue();
            backend.tls.validateCertificateName.Should().BeTrue();
            
            // var yamlSerializer = new YamlDotNet.Serialization.SerializerBuilder()
            //     .Build();
            //
            // var yamlResult = yamlSerializer.Serialize(creatorConfig);
        }
    }
}