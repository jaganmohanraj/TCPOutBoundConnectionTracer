using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace TCP.OutBoundConnection
{
    public class Report
    {
        private static int _currentProcessId;
        private static readonly Dictionary<string, string> HostNames = new Dictionary<string, string>();


        public static ConnectionsReport GenerateActiveConnectionsReport(bool useHostName = false,
            int currentProcessId = 0)
        {
            _currentProcessId = currentProcessId != 0 ? currentProcessId : Process.GetCurrentProcess().Id;

            var connectionsReport = new ConnectionsReport("Active Connection");

            var extendedTcpTable = ManagedIpHelper.GetExtendedTcpTable(true);

            if (_currentProcessId != 0)
            {
                var tcpRowsCurrentProcess = extendedTcpTable.Where(a => a.ProcessId == _currentProcessId).ToList();


                connectionsReport.CurrentProcessBreakDown = PrepareActiveConnectionBreakDown(tcpRowsCurrentProcess);

                connectionsReport.CurrentProcessConnectionDetails =
                    PrepareConnectionDetails(useHostName, tcpRowsCurrentProcess);
            }


            connectionsReport.OverAllBreakDown = PrepareActiveConnectionBreakDown(extendedTcpTable);


            connectionsReport.OverAllConnectionDetails = PrepareConnectionDetails(useHostName, extendedTcpTable);

            return connectionsReport;
        }

        private static List<string> PrepareConnectionDetails(bool useHostName,
            IEnumerable<TcpRow> tcpRowsCurrentProcess)
        {
            return (from tcpRow in tcpRowsCurrentProcess
                    let process = Process.GetProcessById(tcpRow.ProcessId)
                    let Processname = process.ProcessName != "System"
                        ? Path.GetFileName(process.ProcessName)
                        : "  -- unknown component(s) -- System"
                    select
                        $"   LocalEndPoint: {tcpRow.LocalEndPoint} RemoteEndPoint: {GetHostNameFromIpAddress(tcpRow.RemoteEndPoint.Address.ToString(), useHostName)} State: {tcpRow.State} ProcessName: {Processname}({tcpRow.ProcessId})"
                ).ToList();
        }

        private static ActiveConnectionBreakDown PrepareActiveConnectionBreakDown(
            IEnumerable<TcpRow> tcpRowsCurrentProcess)
        {
            var rowsCurrentProcess = tcpRowsCurrentProcess.ToList();
            var activeConnectionBreakDown = new ActiveConnectionBreakDown
            {
                TotalNoOfConnection = rowsCurrentProcess.Count()
            };


            foreach (var tcpState in EnumUtil.GetValues<TcpState>())
            {
                var count = rowsCurrentProcess.Count(a => a.State == tcpState);

                if (count > 0)
                    activeConnectionBreakDown.ConnectionBreakDown.Add(tcpState.ToString(), count);
            }


            return activeConnectionBreakDown;
        }

        private static string GetHostNameFromIpAddress(string ipAdress, bool useHostName = false)
        {
            var machineName = ipAdress;

            if (!useHostName)
                return machineName;


            if (HostNames.ContainsKey(ipAdress))
                return HostNames[ipAdress];
            try
            {
                var hostEntry = Dns.GetHostEntry(ipAdress);

                machineName = hostEntry.HostName;
            }
            catch
            {
                // Machine not found...
            }

            HostNames.Add(ipAdress, machineName);
            return machineName;
        }
    }
}