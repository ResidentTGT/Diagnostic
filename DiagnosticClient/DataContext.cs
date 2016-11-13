using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticClient
{
    internal class DataContext : IDataContext
    {
        private DiagnosticDbContext dbContext = new DiagnosticDbContext();

        private int GetNodeId(string nodeName, string groupName)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            if (CheckNodeExistence(nodeName, groupName))
            {
                int nodeId;
                if (string.IsNullOrWhiteSpace(groupName))
                    nodeId = dbContext.Nodes.Single(g => g.Name == nodeName && g.NodeGroupId == null).Id;
                else
                {
                    int groupId = GetGroupId(groupName.ToUpper());
                    nodeId = dbContext.Nodes.Single(g => g.Name == nodeName && g.NodeGroupId == groupId).Id;
                }
                return nodeId;
            }
            else
                throw new InvalidOperationException("No node with such name and group");
        }

        public bool IsNodeOnline(string nodeName, string groupName, float offlineTimeMs)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            try
            {
                int nodeId = GetNodeId(nodeName, groupName);
                DateTime startTime = DateTime.Now;
                OnlinePeriod result = dbContext.NodesOnlinePeriods.Where(g => g.NodeId == nodeId).OrderByDescending(g => g.Id).FirstOrDefault();
                if (result == null || (startTime.Subtract(result.EndTime).TotalMilliseconds) > offlineTimeMs)
                    return false;
                else
                    return true;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Сhecking online could not be completed");
            }
        }

        public void RewriteNodeOnlinePeriod(string nodeName, string groupName, float offlineTimeMs)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            try
            {
                int nodeId = GetNodeId(nodeName, groupName);
                DateTime startTime = DateTime.Now;
                var nodeOnlinePeriod = dbContext.NodesOnlinePeriods.Where(g => g.NodeId == nodeId).OrderByDescending(g => g.Id).FirstOrDefault();
                nodeOnlinePeriod.EndTime = startTime.AddMilliseconds(offlineTimeMs);
                dbContext.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Rewriting online period could not be completed");
            }
        }

        public void AddNodeOnlinePeriod(string nodeName, string groupName, float offlineTimeMs)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new ArgumentException("Empty node's name");
            try
            {
                int nodeId = GetNodeId(nodeName, groupName);
                DateTime startTime = DateTime.Now;
                DateTime endTime = startTime.AddMilliseconds(offlineTimeMs);
                dbContext.NodesOnlinePeriods.Add(new OnlinePeriod { StartTime = startTime, NodeId = nodeId, EndTime = endTime });
                dbContext.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Adding online period could not be completed");
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

            int nodeId = GetNodeId(nodeName, groupName);
            dbContext.Logs.Add(new Log { NodeId = nodeId, Time = logTime, Level = logLevel, Message = logMessage });
            dbContext.SaveChanges();
        }

        private int GetGroupId(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException("Empty group's name");
            return dbContext.Groups.Single(g => g.Name == groupName).Id;
        }

        public bool CheckNodeExistence(string nodeName, string groupName)
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
