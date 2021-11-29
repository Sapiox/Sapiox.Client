using System;
using System.Collections.Generic;

namespace SapioxClient.API
{
    public interface IConfig
    {
        bool Load { get; set; }
    }
}