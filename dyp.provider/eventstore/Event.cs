using System;

namespace dyp.provider.eventstore
{
    public abstract class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public EventContext Context { get; set; }
        public DateTime Timestamp { get; set; }
        public EventData Data { get; set; }

        public Event(string name, EventContext context, EventData data)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Context = context;
            Timestamp = DateTime.Now;
            Data = data;
        }
    }
}