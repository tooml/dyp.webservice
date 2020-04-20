using dyp.messagehandling.pipeline.messagecontext;

namespace dyp.dyp.messagepipelines.commands.changeoptionscommand
{
    public class ChangeOptionsCommandContextModel : IMessageContext
    {
        public int Tables;
        public int Sets;
        public int Points;
        public int PointsDrawn;
        public bool Drawn;
        public bool Walkover;
    }
}