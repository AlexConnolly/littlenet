﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using littlenet.Connection.Interfaces;
using littlenet.Server.Models;

namespace littlenet.Server.Interfaces
{
    public interface ILittlenetServer
    {
        Task Start(LittlenetServerConfiguration serverConfiguration);
        Task Stop();

        void OnConnected(Action<IConnection> callback);
    }
}
