using dyp.adapter;
using dyp.contracts.messages.commands.createtournament;
using dyp.dyp.events;
using dyp.dyp.events.context;
using dyp.dyp.events.data;
using dyp.messagehandling;
using dyp.messagehandling.pipeline;
using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;
using dyp.provider.eventstore;
using System;
using System.Linq;
using static dyp.dyp.messagepipelines.PersonsContextModel;

namespace dyp.dyp.messagepipelines.commands.createtournamentcommand
{
    public class CreateTournamentCommandProcessor : IMessageProcessor
    {
        private readonly IIdProvider _id_provider;
        private readonly IDateProvider _date_provider;

        public CreateTournamentCommandProcessor(IIdProvider id_provider, IDateProvider date_provider)
        {
            _id_provider = id_provider;
            _date_provider = date_provider;
        }

        public Output Process(IMessage input, IMessageContext model)
        {
            Console.WriteLine("start create tournament process");
            var cmd = input as CreateTournamentCommand;
            var ctx_model = model as CreateTournamentCommandContextModel;

            var tournament_id = _id_provider.Get_new_id().ToString();
            var created = _date_provider.Get_current_date().ToString();

            Console.WriteLine($"id: { tournament_id }, created {created}");

            var tournament_events = Map_tournament(tournament_id, created, cmd);
            var optins_events = Map_options(tournament_id, cmd);
            var player_events = ctx_model.Persons.Where(p => cmd.CompetitorsIds.Contains(p.Id))
                                                        .Select(p => Map_players(tournament_id, p));

            var events = tournament_events.Concat(optins_events).Concat(player_events).ToArray();
            Console.WriteLine($"events count: { events.Count() }");

            return new CommandOutput(new Success(), events);
        }

        private Event[] Map_tournament(string tournament_id, string created, CreateTournamentCommand cmd)
        {
            var tournament_data = new TournamentData()
            {
                Id = tournament_id,
                Name = cmd.Name,
                Created = created
            };

            return new Event[] { new TournamentCreated(nameof(TournamentCreated),
                new TournamentContext(tournament_id, nameof(TournamentContext)), tournament_data) };
        }

        private Event[] Map_options(string tournament_id, CreateTournamentCommand cmd)
        {
            var options_data = new OptionsData()
            {
                Tables = cmd.Tables,
                Sets = cmd.Sets,
                Points = cmd.Points,
                Points_drawn = cmd.PointsDrawn,
                Drawn = cmd.Drawn,
                Fair_lots = cmd.FairLots
            };

            return new Event[] { new OptionsCreated(nameof(TournamentCreated),
                new TournamentContext(tournament_id, nameof(TournamentContext)), options_data) };
        }

        private Event Map_players(string tournament_id, PersonInfo person)
        {
            var player_datas = new PlayerData()
            {
                Player = new Player()
                {
                    Id = person.Id,
                    First_name = person.First_name,
                    Last_name = person.Last_name
                }
            };

            return new PlayersStored(nameof(PlayersStored),
                new TournamentContext(tournament_id, nameof(TournamentContext)), player_datas);
        }
    }
}