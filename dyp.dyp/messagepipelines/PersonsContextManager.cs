using dyp.dyp.events;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines
{
    public class PersonsContextManager<T> : IMessageContextManager where T : PersonsContextModel
    {
        private readonly List<PersonsContextModel.PersonInfo> _persons;

        public PersonsContextManager(IEventStore es)
        {
            _persons = new List<PersonsContextModel.PersonInfo>();
            Update(es.Replay(typeof(PersonStored), typeof(PersonUpdated), typeof(PersonDeleted)));
        }

        public IMessageContext Load(IMessage _)
        {
            var context_model = (T)Activator.CreateInstance(typeof(T));
            context_model.Persons = _persons;

            return context_model;
        }

        public void Update(IEnumerable<Event> events)
            => events.ToList().ForEach(ev => Apply(ev));

        private void Apply(Event ev)
        {
            switch (ev)
            {
                case PersonStored ps:
                    var new_person = Map_person_data(ps.Data as PersonData);
                    _persons.Add(new_person);
                    break;
                case PersonUpdated pu:
                    var person_update = Map_person_data(pu.Data as PersonData);
                    var update = _persons.Single(pers => pers.Id.Equals(person_update.Id));
                    update.First_name = person_update.First_name;
                    update.Last_name = person_update.Last_name;
                    break;
                case PersonDeleted ps:
                    var person_delete_data = ps.Data as PersonDeleteData;
                    var index = _persons.FindIndex(pers => pers.Id.Equals(person_delete_data.Id));
                    _persons.RemoveAt(index);
                    break;
            }
        }

        private PersonsContextModel.PersonInfo Map_person_data(PersonData eventData)
        {
            return new PersonsContextModel.PersonInfo()
            {
                Id = eventData.Person.Id,
                First_name = eventData.Person.First_name,
                Last_name = eventData.Person.Last_name
            };
        }
    }
}