using dyp.dyp.events;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.messagecontext.messagehandling.pipeline.messagecontext;
using dyp.provider.eventstore;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.queries.personavatarsquery
{
    public class PersonAvtarsQueryContextManager : IMessageContextManager
    {
        private readonly IEventStore _es;
        private PersonAvtarsQueryContextModel _ctx_model;

        public PersonAvtarsQueryContextManager(IEventStore es) { _es = es; }

        public IMessageContext Load(IMessage input)
        {
            _ctx_model = new PersonAvtarsQueryContextModel();
            Update(_es.Replay(typeof(PersonAvatarStored), typeof(PersonDeleted)));

            return _ctx_model;
        }

        public void Update(IEnumerable<Event> events) 
            => events.ToList().ForEach(ev => Apply(ev));

        private void Apply(Event ev)
        {
            switch (ev)
            {
                case PersonAvatarStored sc:
                    var avatar_data = ev.Data as PersonAvatarData;
                    var index = _ctx_model.Avatars.FindIndex(av => av.Person_id.Equals(avatar_data.Person_Id));
                    if (index == -1) _ctx_model.Avatars.Add(new PersonAvtarsQueryContextModel.PersonAvatar() 
                    { 
                        Person_id = avatar_data.Person_Id, 
                        Avatar = avatar_data.Avatar 
                    });
                    else _ctx_model.Avatars[index].Avatar = avatar_data.Avatar;
                    break;

                case PersonDeleted sd:
                    var delete_data = ev.Data as PersonDeleteData;
                    var _index = _ctx_model.Avatars.FindIndex(x => x.Person_id.Equals(delete_data.Id));
                    if (_index != -1) _ctx_model.Avatars.RemoveAt(_index);
                    break;
            }
        }
    }
}