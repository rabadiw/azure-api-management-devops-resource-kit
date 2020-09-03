using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Convert;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Converter;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Create;
using Xunit;
using YamlDotNet.Serialization;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Test.Convert
{
    public class BackendCreatorConfigMapperTests
    {
        [Fact]
        public async Task Should_map_backend_template_to_creatorConfig()
        {
            var templatePath = Path.GetFullPath("../../../../apimtemplate/Creator/ExampleGeneratedTemplates");
            var backendPaths = ConverterExtensions.GetBackendsTemplateJson(templatePath);

            var backendJsonFile = backendPaths.First();
            var converter = new BackendCreatorConfigMapper(backendJsonFile);
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
        }
    }

    public class Given_a_valid_backendArm_template_when_converting
    {
        private const string expectedYaml = @"title: myBackend
description: description5308
url: https://backendname2644/
protocol: http
credentials:
  query: 
    sv: 
      - xx
      - bb
  header: 
    x-my-1:
      - val1
      - val2
  authorization: 
    scheme: Basic
    parameter: opensesma
proxy:
  url: http://192.168.1.1:8080
  username: Contoso\admin
  password: opensesame
tls:
  validateCertificateChain: true
  validateCertificateName: true";
        
        [Fact]
        public async Task Should_generate_valid_creatorConfig_yaml()
        {
            var path = Path.GetFullPath("../../../../apimtemplate/Creator/ExampleGeneratedTemplates");
            var sut = new ArmConverter(path);

            var result = await sut.ConvertAsync();
            
            var deserializer = new Deserializer();
            var expectedBackendObject = deserializer.Deserialize<BackendTemplateProperties>(expectedYaml);
            var resultObject = deserializer.Deserialize<BackendTemplateProperties>(result);
            
            resultObject.Should().BeEquivalentTo(expectedBackendObject);
        }
    }
}