using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticClient.Tests
{
    internal class DiagnosticsMock : IDataContext
    {
        public List<Log> Logs = new List<Log>();
        public List<Node> Nodes = new List<Node>();
        public List<Group> Groups = new List<Group>();
        public List<OnlinePeriod> OnlinePeriods = new List<OnlinePeriod>();

        public void AddNodeOnlinePeriod(string nodeName, string groupName, float offlineTimeMs)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException();
            Nodes.Single(n => n.Name == nodeName);
            OnlinePeriods.Add(new OnlinePeriod { NodeId = 2, StartTime = DateTime.Now, EndTime = DateTime.Now.AddMilliseconds(offlineTimeMs) });
        }
        public bool CheckNodeExistence(string nodeName, string groupName)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException();
            if (Nodes.Any(g => g.Name == nodeName && g.NodeGroupId == 1))
                return true;
            return false;
        }

        public bool IsNodeOnline(string nodeName, string groupName, float offlineTimeMs)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException();
            var currentTime = DateTime.Now;
            if (currentTime.Subtract(OnlinePeriods[0].EndTime).TotalMilliseconds > offlineTimeMs)
                return false;
            else
                return true;
        }

        public void RewriteNodeOnlinePeriod(string nodeName, string groupName, float offlineTimeMs)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException();
            OnlinePeriods[0].EndTime = DateTime.Now.AddMilliseconds(offlineTimeMs);
        }

        public void AddLog(string nodeName, string groupName, string logLevel, string logMessage, DateTime logTime)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new InvalidOperationException("Empty node's name");
            if (string.IsNullOrWhiteSpace(logLevel))
                throw new InvalidOperationException("Empty log's level");
            if (string.IsNullOrWhiteSpace(logMessage))
                throw new InvalidOperationException("Empty log's message");

            Logs.Add(new Log { Id = 1, Level = logLevel, Message = logMessage, NodeId = 1, Time = logTime });
        }
    }
}
