using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticClient
{
    internal class DataContext : IDataContext
    {
        private int GetNodeId(string nodeName, string groupName)
        {

            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            using (var dbContext = new DiagnosticDbContext())
            {
                if (CheckNodeExistence(nodeName, groupName))
                {
                    int nodeId;
                    if (string.IsNullOrWhiteSpace(groupName))
                        nodeId = dbContext.Nodes.Single(g => g.Name == nodeName && g.NodeGroupId == null).Id;
                    else
                        nodeId = dbContext.Nodes.Single(g => g.Name == nodeName && g.Group.Name == groupName).Id;

                    return nodeId;
                }
                else
                    throw new InvalidOperationException("No node with such name and group");
            }
        }

        public bool IsNodeOnline(string nodeName, string groupName)
        {

            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            using (var dbContext = new DiagnosticDbContext())
            {
                int nodeId = GetNodeId(nodeName, groupName);
                DateTime now = DateTime.Now;
                OnlinePeriod result = dbContext.NodesOnlinePeriods.Where(g => g.NodeId == nodeId).OrderByDescending(g => g.Id).FirstOrDefault();
                return !((result == null) || (now > result.EndTime));
            }
        }

        public void SetEndTime(string nodeName, string groupName, DateTime endTime)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            using (var dbContext = new DiagnosticDbContext())
            {
                var nodeOnlinePeriod = dbContext.NodesOnlinePeriods.Where(g => g.Node.Name == nodeName && g.Node.Group.Name == groupName).OrderByDescending(g => g.Id).FirstOrDefault();
                nodeOnlinePeriod.EndTime = endTime;
                dbContext.SaveChanges();
            }
        }

        public void AddNodeOnlinePeriod(string nodeName, string groupName, DateTime startTime, DateTime endTime)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            using (var dbContext = new DiagnosticDbContext())
            {
                int nodeId = GetNodeId(nodeName, groupName);
                dbContext.NodesOnlinePeriods.Add(new OnlinePeriod { StartTime = startTime, NodeId = nodeId, EndTime = endTime });
                dbContext.SaveChanges();
            }
        }

        public void AddLog(string nodeName, string groupName, string logLevel, string logMessage, DateTime logTime)
        {

            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            if (string.IsNullOrWhiteSpace(logLevel))
                throw new ArgumentException("Empty log's level");
            if (string.IsNullOrWhiteSpace(logMessage))
                throw new ArgumentException("Empty log's message");
            using (var dbContext = new DiagnosticDbContext())
            {
                int nodeId = GetNodeId(nodeName, groupName);
                dbContext.Logs.Add(new Log { NodeId = nodeId, Time = logTime, Level = logLevel, Message = logMessage });
                dbContext.SaveChanges();
            }
        }

        public bool CheckNodeExistence(string nodeName, string groupName)
        {
            using (var dbContext = new DiagnosticDbContext())
            {
                if (string.IsNullOrWhiteSpace(nodeName))
                    throw new ArgumentException("Empty node's name");
                bool exist = false;
                if (string.IsNullOrWhiteSpace(groupName))
                    exist = dbContext.Nodes.Any(g => g.Name == nodeName && g.NodeGroupId == null);
                else
                    exist = dbContext.Nodes.Any(g => g.Name == nodeName && g.Group.Name == groupName);
                return exist;
            }
        }
    }
}
