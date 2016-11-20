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

        public void AddNodeOnlinePeriod(string nodeName, string groupName, DateTime startTime, DateTime endTime)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            int nodeId = GetNodeId(nodeName, groupName);
            OnlinePeriods.Add(new OnlinePeriod { StartTime = startTime, NodeId = nodeId, EndTime = endTime });
        }

        public bool CheckNodeExistence(string nodeName, string groupName)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException();
            int groupId = GetGroupId(groupName);
            if (Nodes.Any(g => g.Name == nodeName && g.NodeGroupId == groupId))
                return true;
            return false;
        }

        public bool IsNodeOnline(string nodeName, string groupName)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            int nodeId = GetNodeId(nodeName, groupName);
            DateTime now = DateTime.Now;
            OnlinePeriod result = OnlinePeriods.Where(g => g.NodeId == nodeId).OrderByDescending(g => g.Id).FirstOrDefault();
            return !((result == null) || (now>result.EndTime));
        }

        public void SetEndTime(string nodeName, string groupName, DateTime endTime)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            int nodeId = GetNodeId(nodeName, groupName);
            var nodeOnlinePeriod = OnlinePeriods.Where(g => g.NodeId == nodeId).OrderByDescending(g => g.Id).FirstOrDefault();
            nodeOnlinePeriod.EndTime = endTime;
        }

        public void AddLog(string nodeName, string groupName, string logLevel, string logMessage, DateTime logTime)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new InvalidOperationException("Empty node's name");
            if (string.IsNullOrWhiteSpace(logLevel))
                throw new InvalidOperationException("Empty log's level");
            if (string.IsNullOrWhiteSpace(logMessage))
                throw new InvalidOperationException("Empty log's message");
            int nodeId = GetNodeId(nodeName, groupName);
            Logs.Add(new Log { NodeId = nodeId, Time = logTime, Level = logLevel, Message = logMessage });
        }

        private int GetNodeId(string nodeName, string groupName)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            if (CheckNodeExistence(nodeName, groupName))
            {
                int nodeId;
                if (string.IsNullOrWhiteSpace(groupName))
                    nodeId = Nodes.Single(g => g.Name == nodeName && g.NodeGroupId == null).Id;
                else
                {
                    int groupId = GetGroupId(groupName);
                    nodeId = Nodes.Single(g => g.Name == nodeName && g.NodeGroupId == groupId).Id;
                }
                return nodeId;
            }
            else
                throw new InvalidOperationException("No node with such name and group");
        }

        private int GetGroupId(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("Empty group's name");
            return Groups.Single(g => g.Name == groupName).Id;
        }
    }
}
