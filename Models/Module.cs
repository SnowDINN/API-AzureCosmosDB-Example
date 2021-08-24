namespace NorthEducationAPI.Models
{
    using Newtonsoft.Json;

    public class Module
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "audio")]
        public string Audio { get; set; }
    }
}