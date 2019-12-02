using System.Net;
using System.Net.NetworkInformation;

namespace TCP.OutBoundConnection
{
    internal class TcpRow
    {
        #region Constructors

        public TcpRow(IpHelper.TcpRow tcpRow)
        {
            State = tcpRow.state;
            ProcessId = tcpRow.owningPid;

            var localPort = (tcpRow.localPort1 << 8) + tcpRow.localPort2 + (tcpRow.localPort3 << 24) +
                            (tcpRow.localPort4 << 16);
            long localAddress = tcpRow.localAddr;
            LocalEndPoint = new IPEndPoint(localAddress, localPort);

            var remotePort = (tcpRow.remotePort1 << 8) + tcpRow.remotePort2 + (tcpRow.remotePort3 << 24) +
                             (tcpRow.remotePort4 << 16);
            long remoteAddress = tcpRow.remoteAddr;
            RemoteEndPoint = new IPEndPoint(remoteAddress, remotePort);
        }

        #endregion

        #region Private Fields

        #endregion

        #region Public Properties

        public IPEndPoint LocalEndPoint { get; }

        public IPEndPoint RemoteEndPoint { get; }

        public TcpState State { get; }

        public int ProcessId { get; }

        #endregion
    }
}