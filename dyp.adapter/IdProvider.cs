using System;

namespace dyp.adapter
{
    public class IdProvider : IIdProvider
    {
        public Guid Get_new_id() => Guid.NewGuid();
    }
}