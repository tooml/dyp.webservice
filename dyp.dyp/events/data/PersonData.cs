using dyp.provider.eventstore;

namespace dyp.dyp.events.data
{
    public class Person
    {
        public string Id;
        public string First_name;
        public string Last_name;
        public string Image;
    }

    public class PersonData : EventData
    {
        public Person Person;
    }
}