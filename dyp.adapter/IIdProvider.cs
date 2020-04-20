using System;

namespace dyp.adapter
{
    public interface IIdProvider
    {
        Guid Get_new_id();
    }
}
