using dyp.adapter;
using dyp.contracts.data;
using dyp.contracts.messages.commands.createnewround;
using dyp.dyp.domain;
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
using System.Runtime.CompilerServices;
using static dyp.dyp.messagepipelines.commands.createroundcommand.CreateRoundCommandContextModel;

namespace dyp.dyp.messagepipelines.commands.createroundcommand
{
    public class CreateRoundCommandProcessor : IMessageProcessor
    {
        private readonly IIdProvider _id_provider;

        public CreateRoundCommandProcessor(IIdProvider id_provider)
        {
            _id_provider = id_provider;
        }

        public Output Process(IMessage input, IMessageContext model)
        {
            var cmd = input as CreateRoundCommand;
            var ctx_model = model as CreateRoundCommandContextModel;

            var tournament_director = new TournamentDirector();
            var round = tournament_director.New_round(Map(ctx_model));

            var round_id = _id_provider.Get_new_id().ToString();

            var events = new List<Event>();
            events.Add(Map(round, round_id, cmd.TournamentId, ctx_model.Round_count));
            events.AddRange(Map(round.Matches, ctx_model, round_id, cmd.TournamentId));
            events.AddRange(Map(round.Walkover, cmd.TournamentId));
            events.AddRange(Map(round.Matches, cmd.TournamentId));

            return new CommandOutput(new Success(), events.ToArray());
        }

        private CreateRoundArgs Map(CreateRoundCommandContextModel model)
        {
            var players = model.Players.Where(pl => pl.Enabled).Select(pl => new contracts.data.Player()
            {
                Id = pl.Id,
                First_name = pl.First_name,
                Last_name = pl.Last_name,
                Matches = pl.Matches,
                Walkover_played = model.Walkover_player_ids.Count(p => p == pl.Id),
                Strength_amount = pl.Strength
            });

            return new CreateRoundArgs()
            {
                Players = players,
                Tables = model.Tables,
                Fair_lots = model.Fair_lots,
                Previous_teams = model.Previous_teams
            };
        }

        private Event Map(Round round, string round_id, string tournament_id, int round_count)
        {
            RoundData round_data = new RoundData()
            {
                Id = round_id,
                Count = round_count
            };

            return new RoundCreated(nameof(RoundCreated),
                new TournamentContext(tournament_id, nameof(TournamentContext)), round_data);
        }

        private IEnumerable<Event> Map(IEnumerable<contracts.data.Match> matches,
                                        CreateRoundCommandContextModel ctx_model, string round_id, string tournament_id)
        {
            var matches_data = matches.Select(m => new MatchData()
            {
                Id = _id_provider.Get_new_id().ToString(),
                Round_id = round_id,
                Home = new MatchData.Team()
                {
                    Player_one = new events.data.Player()
                    {
                        Id = m.Home.Member_one.Id,
                        First_name = m.Home.Member_one.First_name,
                        Last_name = m.Home.Member_one.Last_name
                    },
                    Player_two = new events.data.Player()
                    {
                        Id = m.Home.Member_two.Id,
                        First_name = m.Home.Member_two.First_name,
                        Last_name = m.Home.Member_two.Last_name
                    }
                },
                Away = new MatchData.Team()
                {
                    Player_one = new events.data.Player()
                    {
                        Id = m.Away.Member_one.Id,
                        First_name = m.Away.Member_one.First_name,
                        Last_name = m.Away.Member_one.Last_name
                    },
                    Player_two = new events.data.Player()
                    {
                        Id = m.Away.Member_two.Id,
                        First_name = m.Away.Member_two.First_name,
                        Last_name = m.Away.Member_two.Last_name
                    }
                },
                Table = m.Table,
                Sets = ctx_model.MatchOption.Sets,
                Drawn = ctx_model.MatchOption.Drawn,
            }).ToList();


            return matches_data.Select(f =>
            {
                return new MatchCreated(nameof(MatchCreated),
                    new TournamentContext(tournament_id, nameof(TournamentContext)), f);
            }).ToList();
        }

        private IEnumerable<Event> Map(IEnumerable<contracts.data.Player> walkover, string tournament_id)
        {
            var walkover_datas = walkover.Select(w => new WalkoverData() { Id = w.Id }).ToList();
            return walkover_datas.Select(w =>
            {
                return new WalkoverPlayed(nameof(WalkoverPlayed),
                    new TournamentContext(tournament_id, nameof(TournamentContext)), w);
            }).ToList();
        }

        private IEnumerable<Event> Map(IEnumerable<contracts.data.Match> matches, string tournament_id)
        {
            var team_datas = matches.Select(m =>
            {
                return new TeamCreatedData[]
                {
                    new TeamCreatedData(){ Player_one_id = m.Home.Member_one.Id, Player_two_id = m.Home.Member_two.Id },
                    new TeamCreatedData(){ Player_one_id = m.Away.Member_one.Id, Player_two_id = m.Away.Member_two.Id }
                };
            });

            return team_datas.SelectMany(datas =>
                                    datas.Select(data =>
                                            new TeamCreated(nameof(TeamCreated),
                                            new TournamentContext(tournament_id, nameof(TournamentContext)), data)));
        }
    }
}