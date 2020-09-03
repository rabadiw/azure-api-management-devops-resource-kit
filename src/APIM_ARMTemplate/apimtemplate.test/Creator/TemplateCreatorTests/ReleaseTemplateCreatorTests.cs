using System.Collections.Generic;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Create;
using Xunit;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Test.Creator
{
    public class ReleaseTemplateCreatorTests
    {
        [Fact]
        public void ShouldCreateReleaseTemplateResourceFromCreatorConfig()
        {
            // arrange
            ReleaseTemplateCreator releaseTemplateCreator = new ReleaseTemplateCreator();
            CreatorConfig creatorConfig = new CreatorConfig() { apis = new List<APIConfig>() };
            APIConfig api = new APIConfig()
            {
                name = "name",
                apiRevision = "2",
                isCurrent = true,
                suffix = "suffix",
                subscriptionRequired = true,
                openApiSpec = "https://petstore.swagger.io/v2/swagger.json",
            };
            creatorConfig.apis.Add(api);

            // act
            string[] dependsOn = new string[] { "dependsOn" };
            ReleaseTemplateResource releaseTemplateResource = releaseTemplateCreator.CreateAPIReleaseTemplateResource(api, dependsOn);

            // assert
            string releaseName = $"";
            Assert.Equal($"[concat(parameters('ApimServiceName'), '/{api.name}/release-revision-{api.apiRevision}')]", releaseTemplateResource.name);
            Assert.Equal(dependsOn, releaseTemplateResource.dependsOn);
            Assert.Equal($"Release created to make revision {api.apiRevision} current.", releaseTemplateResource.properties.notes);
            Assert.Equal($"[resourceId('Microsoft.ApiManagement/service/apis', parameters('ApimServiceName'), '{api.name}')]", releaseTemplateResource.properties.apiId);
        }
    }
}
