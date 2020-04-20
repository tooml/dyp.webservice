using System;
using dyp.provider.eventstore;

namespace dyp.dyp.events.data
{
    public class PersonDeleteData : EventData
    {
        public string Id;
    }
}