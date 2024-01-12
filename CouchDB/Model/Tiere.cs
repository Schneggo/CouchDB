using CouchDB.Driver.Types;
using Newtonsoft.Json;

namespace CouchDB.Model
{
    public class Tiere : CouchDocument
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("name")]

        public string Name { get; set; }
        [JsonProperty("alter")]

        public int Alter { get; set; }
        [JsonProperty("essen")]

        public string Essen { get; set; }
    }
}
