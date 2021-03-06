﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MCQuery
{
    public class Rcon : Connection
    {
        private readonly int _requestId = 1;

        private readonly int _loginType = 3;
        private readonly int _commandType = 2;
        private readonly int _multiType = 0;
        private readonly byte[] _twoBytePad = { 0x00, 0x00 };

        private string _password;
        private bool _isAuthenticated;

        public Rcon(string address, int port, string password) : base(address, port)
        {
            _password = password;
        }

        public bool Login()
        {
            //Int32 = 4 byte array
            //1. Reminder Lenght = int
            //2. Request Id = int
            //3. Type = int
            //4. Payload = byte[]
            //5. 2-byte pad = byte, byte

            List<byte> message = new List<byte>();

            //Reminder = requestId (int32=4) + loginType (int32=4) + twoBytePad (2) => 10
            var reminder = BitConverter.GetBytes(10 + Encoding.UTF8.GetByteCount(_password));
            message.AddRange(reminder);
            message.AddRange(BitConverter.GetBytes(_requestId));
            message.AddRange(BitConverter.GetBytes(_loginType));
            message.AddRange(Encoding.UTF8.GetBytes(_password));
            message.AddRange(_twoBytePad);

            byte[] response = SendByTcp(base.Address, base.Port, message.ToArray());

            foreach (byte item in BitConverter.GetBytes(response.Length))
            {
                if (reminder[0] == item)
                {
                    _isAuthenticated = true;
                    break;
                }
            }

            return _isAuthenticated;
        }

        public bool SendCommand(string command)
        {
            List<byte> message = new List<byte>();

            //Reminder = requestId (int32=4) + commandType (int32=4) + twoBytePad (2) => 10
            var reminder = BitConverter.GetBytes(10 + Encoding.UTF8.GetByteCount(command));
            message.AddRange(reminder);
            message.AddRange(BitConverter.GetBytes(_requestId));
            message.AddRange(BitConverter.GetBytes(_commandType));
            message.AddRange(Encoding.UTF8.GetBytes(command));
            message.AddRange(_twoBytePad);

            byte[] response = SendByTcp(base.Address, base.Port, message.ToArray());
            bool didSucceeed = false;

            foreach (byte item in BitConverter.GetBytes(response.Length))
            {
                if (reminder[0] == item)
                {
                    didSucceeed = true;
                    break;
                }
            }

            return didSucceeed;
        }

        public override bool IsConnected => _isAuthenticated;
    }
}