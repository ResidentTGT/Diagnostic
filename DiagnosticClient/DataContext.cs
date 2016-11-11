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

        IList<Log> IDataContext.Logs
        { get; set; }
        IList<Node> IDataContext.Nodes
        { get; set; }
        IList<Group> IDataContext.Groups
        { get; set; }
        IList<OnlinePeriod> IDataContext.OnlinePeriods
        { get; set; }

        private int GetNodeId(string nodeName, string groupName)
        {
            int nodeId;
            int groupId = GetGroupId(groupName.ToUpper());

            if (string.IsNullOrWhiteSpace(groupName))
                return nodeId = dbContext.Nodes.Single(g => g.Name.ToUpper() == nodeName.ToUpper() && g.NodeGroupId == null).Id;
            else
                return nodeId = dbContext.Nodes.Single(g => g.Name.ToUpper() == nodeName.ToUpper() && g.NodeGroupId == groupId).Id;
        }

        public bool IsNodeOnline(string nodeName, string groupName, float offlineTimeMs)
        {
            DateTime startTime = DateTime.Now;
            int nodeId = GetNodeId(nodeName, groupName);

            OnlinePeriod result;
            result = dbContext.NodesOnlinePeriods.Where(g => g.NodeId == nodeId).OrderByDescending(g => g.Id).FirstOrDefault();

            if (result == null || (startTime.Subtract(result.EndTime).TotalMilliseconds) > offlineTimeMs)
                return false;
            else
                return true;
        }

        public void RewriteNodeOnlinePeriod(string nodeName, string groupName, float offlineTimeMs)
        {
            DateTime startTime = DateTime.Now;
            int nodeId = GetNodeId(nodeName, groupName);
            var nodeOnlinePeriod = dbContext.NodesOnlinePeriods.Where(g => g.NodeId == nodeId).OrderByDescending(g => g.Id).FirstOrDefault();
            nodeOnlinePeriod.EndTime = startTime.AddMilliseconds(offlineTimeMs);
            dbContext.SaveChanges();
        }

        public void AddNodeOnlinePeriod(string nodeName, string groupName, float offlineTimeMs)
        {
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddMilliseconds(offlineTimeMs);
            int nodeId = GetNodeId(nodeName, groupName);
            dbContext.NodesOnlinePeriods.Add(new OnlinePeriod { StartTime = startTime, NodeId = nodeId, EndTime = startTime.AddMilliseconds(offlineTimeMs) });
            dbContext.SaveChanges();
        }

        public void AddLog(string nodeName, string groupName, string logLevel, string logMessage, DateTime logTime)
        {
            int nodeId = GetNodeId(nodeName, groupName);
            dbContext.Logs.Add(new Log { NodeId = nodeId, Time = logTime, Level = logLevel, Message = logMessage });
            dbContext.SaveChanges();
        }

        private int GetGroupId(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
                throw new ArgumentException();
            int groupId;
            return groupId = dbContext.Groups.Single(g => g.Name.ToUpper() == groupName.ToUpper()).Id;
        }

        public bool CheckNode(string nodeName, string groupName)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new InvalidOperationException("Пустое имя пользователя");
            try
            {
                if (string.IsNullOrWhiteSpace(groupName))
                    dbContext.Nodes.Single(g => g.Name.ToUpper() == nodeName.ToUpper() && g.NodeGroupId == null);

                dbContext.Nodes.Single(g => g.Name.ToUpper() == nodeName.ToUpper() && g.Group.Name.ToUpper() == groupName.ToUpper());
            }
            catch (InvalidOperationException) when (dbContext.Nodes.Single(g => g.Name.ToUpper() == nodeName.ToUpper() && g.NodeGroupId == null) == null)
            {
                throw new InvalidOperationException("Пользователя с таким именем не существует");
            }

            catch (InvalidOperationException) when (dbContext.Nodes.Single(g => g.Name.ToUpper() == nodeName.ToUpper() && g.Group.Name.ToUpper() == groupName.ToUpper()) == null)
            {
                throw new InvalidOperationException("Пользователя с таким именем и группой не существует");
            }
            return true;
        }

        public bool CheckLog(string nodeName, string groupName, string logLevel, string logMessage, DateTime logTime)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new InvalidOperationException("Пустое имя пользователя");
            if (string.IsNullOrWhiteSpace(logLevel))
                throw new InvalidOperationException("Не задан уровень лога");
            if (string.IsNullOrWhiteSpace(logMessage))
                throw new InvalidOperationException("Сообщение лога пустое");
            if (string.IsNullOrWhiteSpace(logTime.ToString()))
                throw new InvalidOperationException("Не указано время");
            try
            {
                if (string.IsNullOrWhiteSpace(groupName))
                    dbContext.Nodes.Single(g => g.Name.ToUpper() == nodeName.ToUpper() && g.NodeGroupId == null);
                dbContext.Nodes.Single(g => g.Name.ToUpper() == nodeName.ToUpper() && g.Group.Name.ToUpper() == groupName.ToUpper());
            }
            catch (InvalidOperationException) when (dbContext.Nodes.Single(g => g.Name.ToUpper() == nodeName.ToUpper() && g.NodeGroupId == null) == null)
            {
                throw new InvalidOperationException("Пользователя с таким именем не существует");
            }

            catch (InvalidOperationException) when (dbContext.Nodes.Single(g => g.Name.ToUpper() == nodeName.ToUpper() && g.Group.Name.ToUpper() == groupName.ToUpper()) == null)
            {
                throw new InvalidOperationException("Пользователя с таким именем и группой не существует");
            }
            return true;
        }
    }
}
