﻿using dyp.provider.eventstore;

namespace dyp.dyp.events
{
    public class PersonMatchStatsDeleted : Event
    {
        public PersonMatchStatsDeleted(string name, EventContext context, EventData data) : base(name, context, data) { }
    }
}