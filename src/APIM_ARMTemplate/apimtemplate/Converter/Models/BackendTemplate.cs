using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Convert.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class ApimServiceName
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Parameters
    {
        [JsonProperty("ApimServiceName")]
        public ApimServiceName ApimServiceName { get; set; }
    }

    public class Header
    {
        [JsonProperty("x-functions-key")]
        public List<string> XFunctionsKey { get; set; }
    }

    public class Credentials
    {
        [JsonProperty("header")]
        public Header Header { get; set; }
    }

    public class Properties
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("resourceId")]
        public string ResourceId { get; set; }

        [JsonProperty("credentials")]
        public Credentials Credentials { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }
    }

    public class Resource
    {
        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("apiVersion")]
        public string ApiVersion { get; set; }
    }

    public class BackendTemplate
    {
        [JsonProperty("$schema")]
        public string Schema { get; set; }

        [JsonProperty("contentVersion")]
        public string ContentVersion { get; set; }

        [JsonProperty("parameters")]
        public Parameters Parameters { get; set; }

        [JsonProperty("resources")]
        public List<Resource> Resources { get; set; }
    }
}