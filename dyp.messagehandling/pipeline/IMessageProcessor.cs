using dyp.messagehandling.pipeline.messagecontext;
using dyp.messagehandling.pipeline.processoroutput;

namespace dyp.messagehandling.pipeline
{
    public interface IMessageProcessor
    {
        Output Process(IMessage input, IMessageContext model);
    }
}