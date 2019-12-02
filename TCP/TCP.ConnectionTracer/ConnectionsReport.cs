using System.Collections.Generic;

namespace TCP.OutBoundConnection
{
    public class ConnectionsReport
    {
        public ActiveConnectionBreakDown CurrentProcessBreakDown;

        public List<string> CurrentProcessConnectionDetails;
        public ActiveConnectionBreakDown OverAllBreakDown;
        public List<string> OverAllConnectionDetails;
        public string Type;

        public ConnectionsReport(string type)
        {
            Type = type;
        }
    }

    public class ActiveConnectionBreakDown
    {
        public Dictionary<string, int> ConnectionBreakDown = new Dictionary<string, int>();

        public int TotalNoOfConnection;
    }
}