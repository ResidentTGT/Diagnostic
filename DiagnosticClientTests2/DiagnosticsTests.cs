using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiagnosticClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticClient.Tests
{
    [TestClass()]
    public class DiagnosticsTests
    {


        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Empty node's name or log message or log level")]
        public void SaveLogWhenEmptyArgumentsTest()
        {
            var mock = new DiagnosticsMock();
            float pingIntervalMs = 5000;
            string nodeName = "Any";
            string groupName = "Any";
            string logLevel = "";
            string logMessage = "Any";
            DateTime logTime = DateTime.Now;
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            var dig = new Diagnostics(pingIntervalMs, mock);

            dig.SaveLog(nodeName, groupName, logLevel, logMessage, logTime);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException), "No node with such name and group")]
        public void SaveLogWhenNotExistingNodeTest()
        {
            var mock = new DiagnosticsMock();
            float pingIntervalMs = 5000;
            string nodeName = "Any";
            string groupName = "Any";
            string logLevel = "Any";
            string logMessage = "Any";
            DateTime logTime = DateTime.Now;
            mock.Nodes.Add(new Node { Id = 1, Name = "Wrong", NodeGroupId = 1 });
            var dig = new Diagnostics(pingIntervalMs, mock);
            dig.SaveLog(nodeName, groupName, logLevel, logMessage, logTime);
        }

        [TestMethod()]
        public void HandlePingWhenNodeOfflineTest()
        {
            var mock = new DiagnosticsMock();
            float pingIntervalMs = 5000;
            int subFromRealTimeMsToEnd = 50000;
            int subFromRealTimeMsToStart = 500000;
            string nodeName = "Any";
            string groupName = "Any";
            var etime = new TimeSpan(0, 0, 0, 0, subFromRealTimeMsToEnd);
            var stime = new TimeSpan(0, 0, 0, 0, subFromRealTimeMsToStart);
            var endTime = DateTime.Now.Subtract(etime);
            var startTime = DateTime.Now.Subtract(stime);
            var dig = new Diagnostics(pingIntervalMs, mock);

            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            mock.OnlinePeriods.Add(new OnlinePeriod { Id = 1, NodeId = 1, StartTime = startTime, EndTime = endTime });
            dig.HandlePing(nodeName, groupName);

            Assert.AreEqual(mock.OnlinePeriods.Count, 2);
        }

        [TestMethod()]
        public void HandlePingWhenNodeOnlineTest()
        {
            var mock = new DiagnosticsMock();
            float pingIntervalMs = 50000;
            int subFromRealTimeMsToEnd = 50000;
            int subFromRealTimeMsToStart = 500000;
            string nodeName = "Any";
            string groupName = "Any";
            var etime = new TimeSpan(0, 0, 0, 0, subFromRealTimeMsToEnd);
            var stime = new TimeSpan(0, 0, 0, 0, subFromRealTimeMsToStart);
            var endTime = DateTime.Now.Subtract(etime);
            var startTime = DateTime.Now.Subtract(stime);
            var dig = new Diagnostics(pingIntervalMs, mock);

            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            mock.OnlinePeriods.Add(new OnlinePeriod { Id = 1, NodeId = 1, StartTime = startTime, EndTime = endTime });
            dig.HandlePing(nodeName, groupName);

            Assert.AreEqual(mock.OnlinePeriods.Count, 1);
            Assert.AreNotEqual(mock.OnlinePeriods[0].EndTime, endTime);
            Assert.AreEqual(mock.OnlinePeriods[0].StartTime, startTime);
        }
       
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Empty node's name")]
        public void HandlePingWhenEmptyNodeTest()
        {
            var mock = new DiagnosticsMock();
            float pingIntervalMs = 50000;
            int subFromRealTimeMsToEnd = 50000;
            int subFromRealTimeMsToStart = 500000;
            string nodeName = "";
            string groupName = "Any";
            var etime = new TimeSpan(0, 0, 0, 0, subFromRealTimeMsToEnd);
            var stime = new TimeSpan(0, 0, 0, 0, subFromRealTimeMsToStart);
            var endTime = DateTime.Now.Subtract(etime);
            var startTime = DateTime.Now.Subtract(stime);
            var dig = new Diagnostics(pingIntervalMs, mock);

            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            mock.OnlinePeriods.Add(new OnlinePeriod { Id = 1, NodeId = 1, StartTime = startTime, EndTime = endTime });
            dig.HandlePing(nodeName, groupName);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException), "No node with such name and group")]
        public void HandlePingWhenNotExistingNodeTest()
        {
            var mock = new DiagnosticsMock();
            float pingIntervalMs = 50000;
            int subFromRealTimeMsToEnd = 50000;
            int subFromRealTimeMsToStart = 500000;
            string nodeName = "Any";
            string groupName = "Any";
            var etime = new TimeSpan(0, 0, 0, 0, subFromRealTimeMsToEnd);
            var stime = new TimeSpan(0, 0, 0, 0, subFromRealTimeMsToStart);
            var endTime = DateTime.Now.Subtract(etime);
            var startTime = DateTime.Now.Subtract(stime);
            var dig = new Diagnostics(pingIntervalMs, mock);

            mock.Nodes.Add(new Node { Id = 1, Name = "Wrong", NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            mock.OnlinePeriods.Add(new OnlinePeriod { Id = 1, NodeId = 1, StartTime = startTime, EndTime = endTime });
            dig.HandlePing(nodeName, groupName);
        }
    }
}