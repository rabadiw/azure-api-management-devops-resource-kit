using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Convert;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;
using YamlDotNet.Serialization;

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
            backend.credentials.header.Should()
                .BeEquivalentTo(JObject.Parse(@"{""x-functions-key"": [ ""{{dts-restore-cosmos-query-key}}""]}"));


            //var yamlSerializer = new YamlDotNet.Serialization.SerializerBuilder()
            //    .WithTypeResolver()
            //    .Build();

            //var yamlResult = yamlSerializer.Serialize(creatorConfig);
        }
    }

    public class JArrayTypeResolver : ITypeResolver
    {
        public Type Resolve(Type staticType, object? actualValue)
        {
            if (actualValue is JArray actualJArrayValue)
            {
                return typeof(List<object>);
            }

            return actualValue?.GetType();
        }
    }
}