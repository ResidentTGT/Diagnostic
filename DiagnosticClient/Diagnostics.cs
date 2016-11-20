using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace DiagnosticClient
{
    public class Diagnostics
    {
        private readonly int offlineTimeMs;
        private IDataContext _context;

        public Diagnostics(int pingIntervalMs, IDataContext context)
        {
            _context = context;
            offlineTimeMs = 2 * pingIntervalMs;
        }

        public Diagnostics(int pingIntervalMs) : this(pingIntervalMs, new DataContext())
        {
        }

        public void HandlePing(string nodeName, string groupName)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            var ctx = _context;
            if (ctx.CheckNodeExistence(nodeName, groupName))
            {
                if (ctx.IsNodeOnline(nodeName, groupName))
                {
                    DateTime endTime = DateTime.Now.AddMilliseconds(offlineTimeMs);
                    ctx.SetEndTime(nodeName, groupName, endTime);
                }
                else
                {
                    DateTime startTime = DateTime.Now;
                    DateTime endTime = startTime.AddMilliseconds(offlineTimeMs);
                    ctx.AddNodeOnlinePeriod(nodeName, groupName, startTime,endTime);
                }
            }
            else
                throw new InvalidOperationException("No node with such name and group");
        }

        public void SaveLog(string nodeName, string groupName, string logLevel, string logMessage, DateTime logTime)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            if (string.IsNullOrWhiteSpace(logLevel))
                throw new ArgumentException("Empty log's level");
            if (string.IsNullOrWhiteSpace(logMessage))
                throw new ArgumentException("Empty log's message");

            var ctx = _context;
            if (ctx.CheckNodeExistence(nodeName, groupName))
                ctx.AddLog(nodeName, groupName, logLevel, logMessage, logTime);
            else
                throw new InvalidOperationException("No node with such name and group");
        }
    }
}
