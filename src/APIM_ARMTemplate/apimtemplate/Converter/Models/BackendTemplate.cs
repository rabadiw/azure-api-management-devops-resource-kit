using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Convert
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

    public class Query
    {
        [JsonProperty("sv")]
        public List<string> Sv { get; set; }
    }

    //public class Header 
    //{
    //    [JsonProperty("x-my-1")]
    //    public List<string> XMy1 { get; set; }
    //}

    public class Authorization
    {
        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        [JsonProperty("parameter")]
        public string Parameter { get; set; }
    }

    public class Credentials
    {
        [JsonProperty("query")]
        public Query Query { get; set; }

        [JsonProperty("header")]
        public Dictionary<string, object> Header { get; set; }

        [JsonProperty("authorization")]
        public Authorization Authorization { get; set; }
    }

    public class Proxy
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class Tls
    {
        [JsonProperty("validateCertificateChain")]
        public bool ValidateCertificateChain { get; set; }

        [JsonProperty("validateCertificateName")]
        public bool ValidateCertificateName { get; set; }
    }

    public class Properties
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("credentials")]
        public Credentials Credentials { get; set; }

        [JsonProperty("proxy")]
        public Proxy Proxy { get; set; }

        [JsonProperty("tls")]
        public Tls Tls { get; set; }

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

        [JsonProperty("dependsOn")]
        public List<object> DependsOn { get; set; }
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