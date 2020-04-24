using dyp.contracts.messages.commands.addplayer;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using dyp.provider.eventstore;
using System.Collections.Generic;
using System.Linq;

namespace dyp.dyp.messagepipelines.commands.changeplayerscommand
{
    public class ChangePlayersCommandProcessor : IMessageProcessor
    {
        public Output Process(IMessage input, IMessageContext model)
        {
            var cmd = input as ChangePlayersCommand;
            var ctx_model = model as ChangePlayersCommandContextModel;

            var all_model_player_ids = ctx_model.Tournament_Players.Select(player => player.Id).ToList();

            var disabled_model_player_ids = ctx_model.Tournament_Players.Where(player => !player.Enabled)
                                                                        .Select(player => player.Id).ToList();


            var players_create = Extract_new_players(all_model_player_ids, cmd.PlayerIds).ToList();
            var players_to_disable = Extract_players_to_disable(all_model_player_ids, cmd.PlayerIds).ToList();
            var players_to_enable = Extract_players_to_enable(disabled_model_player_ids, cmd.PlayerIds).ToList();

            var persons = ctx_model.Persons.Where(person => players_create.Contains(person.Id));

            var events = new List<Event>();
            events.AddRange(Map_new_players(persons, cmd.TournamentId));
            events.AddRange(Map_player_change(players_to_disable, false, cmd.TournamentId));
            events.AddRange(Map_player_change(players_to_enable, true, cmd.TournamentId));

            return new CommandOutput(new Success(), events.ToArray());
        }

        private IEnumerable<string> Extract_new_players(IEnumerable<string> players_in_model,
                                                       IEnumerable<string> players_in_cmd)
        {
            return players_in_cmd.Where(p_id => !players_in_model.Contains(p_id));
        }

        private IEnumerable<string> Extract_players_to_disable(IEnumerable<string> players_in_model,
                                                              IEnumerable<string> players_in_cmd)
        {
            return players_in_model.Where(p_id => !players_in_cmd.Contains(p_id));
        }

        private IEnumerable<string> Extract_players_to_enable(IEnumerable<string> disabled_players_in_model,
                                                             IEnumerable<string> players_in_cmd)
        {
            return disabled_players_in_model.Where(p_id => players_in_cmd.Contains(p_id));
        }

        private IEnumerable<Event> Map_new_players(IEnumerable<ChangePlayersCommandContextModel.Person> persons, string tournament_id)
        {
            return persons.Select(p =>
            {
                var data = new PlayerData()
                {
                    Player = new Player()
                    {
                        Id = p.Id,
                        First_name = p.First_name,
                        Last_name = p.Last_name
                    }
                };

                return new PlayersStored(nameof(PlayersStored),
                   new TournamentContext(tournament_id, nameof(TournamentContext)), data);
            });
        }

        private IEnumerable<Event> Map_player_change(IEnumerable<string> player_ids, bool enable_player, string tournament_id)
        {
            return player_ids.Select(id =>
            {
                var data = new PlayerActivityData() { Player_id = id, Activ = enable_player };

                return new PlayerActivityChanged(nameof(PlayerActivityChanged),
                   new TournamentContext(tournament_id, nameof(TournamentContext)), data);
            });
        }
    }
}