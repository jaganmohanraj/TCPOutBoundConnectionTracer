using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TCP.OutBoundConnection
{
    internal static class ManagedIpHelper
    {
        #region Public Methods

        public static TcpTable GetExtendedTcpTable(bool sorted)
        {
            var tcpRows = new List<TcpRow>();

            var tcpTable = IntPtr.Zero;
            var tcpTableLength = 0;

            if (IpHelper.GetExtendedTcpTable(tcpTable, ref tcpTableLength, sorted, IpHelper.AfInet,
                    IpHelper.TcpTableType.OwnerPidAll, 0) != 0)
                try
                {
                    tcpTable = Marshal.AllocHGlobal(tcpTableLength);
                    if (IpHelper.GetExtendedTcpTable(tcpTable, ref tcpTableLength, true, IpHelper.AfInet,
                            IpHelper.TcpTableType.OwnerPidAll, 0) == 0)
                    {
                        var table = (IpHelper.TcpTable) Marshal.PtrToStructure(tcpTable, typeof(IpHelper.TcpTable));

                        var rowPtr = (IntPtr) ((long) tcpTable + Marshal.SizeOf(table.length));
                        for (var i = 0; i < table.length; ++i)
                        {
                            tcpRows.Add(
                                new TcpRow((IpHelper.TcpRow) Marshal.PtrToStructure(rowPtr, typeof(IpHelper.TcpRow))));
                            rowPtr = (IntPtr) ((long) rowPtr + Marshal.SizeOf(typeof(IpHelper.TcpRow)));
                        }
                    }
                }
                finally
                {
                    if (tcpTable != IntPtr.Zero) Marshal.FreeHGlobal(tcpTable);
                }

            return new TcpTable(tcpRows);
        }

        #endregion
    }
}