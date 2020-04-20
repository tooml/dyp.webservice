
namespace dyp.messagehandling.pipeline.processoroutput
{
    public class NotificationOutput : Output
    {
        public Command[] Commands { get; }

        public NotificationOutput(Command[] commands) => Commands = commands;
    }
}