using System;
using TCP.OutBoundConnection;


namespace TCP.OutBoundConnectionFinderConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ShowActiveTcpConnections();
        }


        private static void ShowActiveTcpConnections()
        {
            Console.WriteLine("Active TCP Connections");


            Console.WriteLine("Active Connections");
            Console.WriteLine();

            Console.WriteLine(
                "Info leave Blank for All ,  use 0 to auto pick  current process ID else type the actual process id/pid....");
            Console.WriteLine("Enter Process ID: ");
            var readLine = Console.ReadLine();
            var connectionID = 99999999;
            if (!string.IsNullOrEmpty(readLine))
                int.TryParse(readLine, out connectionID);

            if (connectionID == 0)
                connectionID = 99999999;
            var generateActiveConnectionsReport =Report.GenerateActiveConnectionsReport(currentProcessId: connectionID);


            if (connectionID != 99999999)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"  CurrentProcessBreakDown TotalNoOfConnection : {generateActiveConnectionsReport.CurrentProcessBreakDown.TotalNoOfConnection}");
                Console.WriteLine();
                Console.WriteLine("  ConnectionBreakDown:-");
                Console.WriteLine();

                foreach (var keyValuePair in generateActiveConnectionsReport.CurrentProcessBreakDown
                    .ConnectionBreakDown)
                    Console.WriteLine($"TCPState: {keyValuePair.Key}  Count: {keyValuePair.Value}");

                Console.WriteLine();


                Console.WriteLine("  CurrentProcess Detail :- ");
                Console.WriteLine();

                foreach (var str in generateActiveConnectionsReport.CurrentProcessConnectionDetails)
                    Console.WriteLine(str);
            }

            Console.WriteLine();
            Console.WriteLine(
                $"  TotalNoOfConnection : {generateActiveConnectionsReport.OverAllBreakDown.TotalNoOfConnection}");
            Console.WriteLine();
            Console.WriteLine("  ConnectionBreakDown:-");
            Console.WriteLine();

            foreach (var keyValuePair in generateActiveConnectionsReport.OverAllBreakDown.ConnectionBreakDown)
                Console.WriteLine($"  TCPState: {keyValuePair.Key}  Count: {keyValuePair.Value}");

            Console.WriteLine();
            Console.WriteLine("  CurrentProcess Detail :- ");
            Console.WriteLine();

            foreach (var str in generateActiveConnectionsReport.OverAllConnectionDetails) Console.WriteLine(str);

            Console.WriteLine();


            Console.Write("{0}Press any key to continue...", Environment.NewLine);
            Console.ReadKey();
        }
    }
}