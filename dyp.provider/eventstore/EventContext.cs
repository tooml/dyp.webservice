
namespace dyp.provider.eventstore
{
    public abstract class EventContext
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public EventContext(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}