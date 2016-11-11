using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticClient.Tests
{
    internal class DiagnosticsMock : IDataContext
    {
        public List<Log> NodeLogs = new List<Log>();
        public List<Node> Nodess = new List<Node>();
        public List<Group> NodeGroups = new List<Group>();
        public List<OnlinePeriod> NodeOnlinePeriods = new List<OnlinePeriod>();


        public IList<Log> Logs
        {
            get
            {
                return NodeLogs;
            }
            set
            {
            }
        }
        public IList<Node> Nodes
        {
            get
            {
                return Nodess;
            }
            set
            {
            }
        }
        public IList<Group> Groups
        {
            get
            {
                return NodeGroups;
            }
            set
            {
            }
        }
        public IList<OnlinePeriod> OnlinePeriods
        {
            get
            {
                return NodeOnlinePeriods;
            }
            set
            {
            }
        }

        public void AddNodeOnlinePeriod(string nodeName, string groupName, float offlineTimeMs)
        {
            OnlinePeriods.Add(new OnlinePeriod { NodeId = 2, StartTime = DateTime.Now, EndTime = DateTime.Now.AddMilliseconds(offlineTimeMs) });
        }
        public bool CheckNode(string nodeName, string groupName)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
                throw new InvalidOperationException("Пустое имя пользователя");
            return true;
        }

        public bool IsNodeOnline(string nodeName, string groupName, float offlineTimeMs)
        {

            if (OnlinePeriods[0].StartTime.Subtract(OnlinePeriods[0].EndTime).TotalMilliseconds > offlineTimeMs)
                return false;
            else
                return true;
        }

        public void RewriteNodeOnlinePeriod(string nodeName, string groupName, float offlineTimeMs)
        {
            OnlinePeriods[0].EndTime = DateTime.Now;
        }

        public void AddLog(string nodeName, string groupName, string logLevel, string logMessage, DateTime logTime)
        {
            Logs.Add(new Log { Id = 1, Level = logLevel, Message = logMessage, NodeId = 1, Time = logTime });
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
            return true;
        }
    }
}
