using Newtonsoft.Json;

namespace dyp.provider.eventstore
{
    static class EventSerialization
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All
        };

        public static string Serialize(Event e) => JsonConvert.SerializeObject(e, Settings);

        public static Event Deserialize(string e) => (Event)JsonConvert.DeserializeObject(e, Settings);
    }
}
