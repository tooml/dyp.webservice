using dyp.messagehandling.pipeline.messagecontext;
using System.Linq;

namespace dyp.dyp.messagepipelines.commands.matchresultcommand
{
    public class StoreMatchResultCommandContextModel : IMessageContext
    {
        public string Tournament_id;
        public string Match_id;
        public string[] Home_player;
        public string[] Away_player;

        public int Home_strength;
        public int Away_strength;

        public void Update_strength(string player_id, int strengt_amount)
        {
            if (Home_player.Contains(player_id)) Home_strength += strengt_amount;
            if (Away_player.Contains(player_id)) Away_strength += strengt_amount;
        }
    }
}