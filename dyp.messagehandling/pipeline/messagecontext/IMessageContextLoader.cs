﻿
namespace dyp.messagehandling.pipeline.messagecontext
{
    public interface IMessageContextLoader
    {
        IMessageContext Load(IMessage input);
    }
}