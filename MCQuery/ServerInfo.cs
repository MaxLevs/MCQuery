﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCQuery
{
    public class ServerInfo
    {
        public string Address { get; set; }
        public string Port { get; set; }

        public string Motd { get; set; }
        public string GameType { get; set; }
        public string Map { get; set; }
        public string Plugins { get; set; }
        public string Version { get; set; }

        public int PlayerCount { get; set; }
        public int MaxPlayers { get; set; }
        public string[] PlayerList { get; set; }

        public bool IsOnline { get; set; }

        public ServerInfo(bool isOnline)
        {
            IsOnline = isOnline;
        }
    }
}
