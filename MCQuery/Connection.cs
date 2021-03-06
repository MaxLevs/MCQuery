﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MCQuery
{
	public abstract class Connection : IDisposable
	{
		private string _address = "";
		private int _port = 0;

	    private UdpClient _udpClient;
		private TcpClient _tcpClient;

	    protected Connection(string address, int port)
		{
			_address = address;
			_port = port;
		}

	    public abstract bool IsConnected { get; }

	    public string Address => _address;
	    public int Port => _port;

        protected byte[] SendByUdp(string address, int port, byte[] data)
		{
			try
			{
				//Check if address is a domain name.
				if (IsDomainAddress(address))
				{
					address = GetIpFromDomain(address);
				}

				if (_udpClient == null)
				{
					//Set up UDP Client
					_udpClient = new UdpClient();
					_udpClient.Connect(address, port);
					_udpClient.Client.SendTimeout = 10000; //Timeout after 10 seconds
					_udpClient.Client.ReceiveTimeout = 10000; //Timeout after 10 seconds
				}

				_udpClient.Send(data, data.Length);

				IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
				byte[] receiveData = _udpClient.Receive(ref remoteIpEndPoint);

				if (receiveData.Length == 0)
				{
					return new byte[] { };
				}
			    return receiveData;
			}
			catch (SocketException exception)
			{
				Console.WriteLine("SocketException: {0}", exception.Message);
				throw;
			}
		}

		protected string GetIpFromDomain(string address)
		{
			IPAddress[] addresses = Dns.GetHostAddresses(address);

			return addresses[0].ToString();
		}

		protected bool IsDomainAddress(string address)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(address);

                if (hostEntry.HostName == address)
                {
                    return true;
                }
                return false;
            }
            catch (SocketException exception)
            {
                throw new Exception("Exception: " + exception.Message);
            }
        }

		protected byte[] SendByTcp(string address, int port, byte[] data)
        {
            try
            {
                if (_tcpClient == null)
                {
                    _tcpClient = new TcpClient(address, port)
                    {
                        ReceiveTimeout = 5000
                    };
                }

                NetworkStream networkStream = _tcpClient.GetStream();

                byte[] buffer = new byte[1024];

                networkStream.Write(data, 0, data.Length);
                int byteCount = networkStream.Read(buffer, 0, buffer.Length);

                // return BitConverter.GetBytes(byteCount); // ?? What??

                var res = new byte[byteCount];
                Buffer.BlockCopy(buffer, 0, res, 0, byteCount);
                return res;
            }
            catch (SocketException exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

		public virtual void Close()
        {
            _udpClient?.Close();
            _tcpClient?.Close();

            Dispose();
        }

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}



				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
			    _tcpClient = null;
			    _udpClient = null;

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~Connection() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
