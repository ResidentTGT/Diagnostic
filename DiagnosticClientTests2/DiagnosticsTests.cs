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
        [ExpectedException(typeof(ArgumentException), "Log level isn't empty")]
        public void SaveLogWhenEmptyLogLevelTest()
        {
            var mock = new DiagnosticsMock();
            int pingIntervalMs = 5000;
            string nodeName = "Any";
            string groupName = "Any";
            string logLevel = "";
            string logMessage = "Any";
            DateTime logTime = DateTime.Now;
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            var dig = new Diagnostics(pingIntervalMs, mock);

            dig.SaveLog(nodeName, groupName, logLevel, logMessage, logTime);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Log message isn't empty")]
        public void SaveLogWhenEmptyLogMessageTest()
        {
            var mock = new DiagnosticsMock();
            int pingIntervalMs = 5000;
            string nodeName = "Any";
            string groupName = "Any";
            string logLevel = "Any";
            string logMessage = "";
            DateTime logTime = DateTime.Now;
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            var dig = new Diagnostics(pingIntervalMs, mock);

            dig.SaveLog(nodeName, groupName, logLevel, logMessage, logTime);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Node name isn't empty")]
        public void SaveLogWhenEmptyNodeNameTest()
        {
            var mock = new DiagnosticsMock();
            int pingIntervalMs = 5000;
            string nodeName = "";
            string groupName = "Any";
            string logLevel = "Any";
            string logMessage = "Any";
            DateTime logTime = DateTime.Now;
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            var dig = new Diagnostics(pingIntervalMs, mock);

            dig.SaveLog(nodeName, groupName, logLevel, logMessage, logTime);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Node name isn't empty")]
        public void SaveLogWhenNULLNodeNameTest()
        {
            var mock = new DiagnosticsMock();
            int pingIntervalMs = 5000;
            string nodeName = null;
            string groupName = "Any";
            string logLevel = "Any";
            string logMessage = "Any";
            DateTime logTime = DateTime.Now;
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            var dig = new Diagnostics(pingIntervalMs, mock);

            dig.SaveLog(nodeName, groupName, logLevel, logMessage, logTime);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException), "Such group exists")]
        public void SaveLogWhenWrongGroupNameTest()
        {
            var mock = new DiagnosticsMock();
            int pingIntervalMs = 5000;
            string nodeName = "Any";
            string groupName = "Any";
            string logLevel = "Any";
            string logMessage = "Any";
            DateTime logTime = DateTime.Now;
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            var dig = new Diagnostics(pingIntervalMs, mock);

            dig.SaveLog(nodeName, "Wrong", logLevel, logMessage, logTime);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException), "Node with such name and group exists")]
        public void SaveLogWhenNotExistingNodeTest()
        {
            var mock = new DiagnosticsMock();
            int pingIntervalMs = 5000;
            string nodeName = "Any";
            string groupName = "Any";
            string logLevel = "Any";
            string logMessage = "Any";
            DateTime logTime = DateTime.Now;
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            var dig = new Diagnostics(pingIntervalMs, mock);

            dig.SaveLog("Wrong", groupName, logLevel, logMessage, logTime);
        }

        [TestMethod()]
        public void SaveLogWhenAllRightTest()
        {
            var mock = new DiagnosticsMock();
            int pingIntervalMs = 5000;
            string nodeName = "Any";
            string groupName = "Any";
            string logLevel = "Any";
            string logMessage = "Any";
            DateTime logTime = DateTime.Now;
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            var dig = new Diagnostics(pingIntervalMs, mock);

            dig.SaveLog(nodeName, groupName, logLevel, logMessage, logTime);

            Assert.AreEqual(mock.Logs[0].Level, logLevel);
            Assert.AreEqual(mock.Logs[0].Message, logMessage);
            Assert.AreEqual(mock.Logs[0].Time, logTime);
            Assert.AreEqual(mock.Logs[0].NodeId, mock.Nodes[0].Id);
        }

        [TestMethod()]
        public void HandlePingWhenNodeOfflineTest()
        {
            var mock = new DiagnosticsMock();
            int pingIntervalMs = 5000;
            string nodeName = "Any";
            string groupName = "Any";
            var endTime = DateTime.Now.AddMilliseconds(-1000);
            var startTime = DateTime.Now.AddMilliseconds(-2000);
            var dig = new Diagnostics(pingIntervalMs, mock);
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            mock.OnlinePeriods.Add(new OnlinePeriod { Id = 1, NodeId = 1, StartTime = startTime, EndTime = endTime });

            dig.HandlePing(nodeName, groupName);

            Assert.AreEqual(mock.OnlinePeriods.Count, 2);
            Assert.AreEqual(mock.OnlinePeriods[0].NodeId, mock.OnlinePeriods[1].NodeId);
        }

        [TestMethod()]
        public void HandlePingWhenNodeOnlineTest()
        {
            var mock = new DiagnosticsMock();
            int pingIntervalMs = 5000;
            string nodeName = "Any";
            string groupName = "Any";
            var endTime = DateTime.Now.AddMilliseconds(5000);
            var startTime = DateTime.Now.AddMilliseconds(-5000);
            var dig = new Diagnostics(pingIntervalMs, mock);
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            mock.OnlinePeriods.Add(new OnlinePeriod { Id = 1, NodeId = 1, StartTime = startTime, EndTime = endTime });

            dig.HandlePing(nodeName, groupName);

            Assert.AreEqual(mock.OnlinePeriods.Count, 1);
            Assert.AreEqual(mock.OnlinePeriods[0].StartTime, startTime);
            Assert.AreNotEqual(mock.OnlinePeriods[0].EndTime, endTime);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Node name is not empty")]
        public void HandlePingWhenEmptyNodeTest()
        {
            var mock = new DiagnosticsMock();
            int pingIntervalMs = 5000;
            string nodeName = "";
            string groupName = "Any";
            var endTime = DateTime.Now.AddMilliseconds(5000);
            var startTime = DateTime.Now.AddMilliseconds(-5000);
            var dig = new Diagnostics(pingIntervalMs, mock);
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            mock.OnlinePeriods.Add(new OnlinePeriod { Id = 1, NodeId = 1, StartTime = startTime, EndTime = endTime });

            dig.HandlePing(nodeName, groupName);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Node name is not null")]
        public void HandlePingWhenNULLNodeTest()
        {
            var mock = new DiagnosticsMock();
            int pingIntervalMs = 5000;
            string nodeName = null;
            string groupName = "Any";
            var endTime = DateTime.Now.AddMilliseconds(5000);
            var startTime = DateTime.Now.AddMilliseconds(-5000);
            var dig = new Diagnostics(pingIntervalMs, mock);
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            mock.OnlinePeriods.Add(new OnlinePeriod { Id = 1, NodeId = 1, StartTime = startTime, EndTime = endTime });

            dig.HandlePing(nodeName, groupName);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException), "Node with such name and group exist")]
        public void HandlePingWhenNotExistingNodeTest()
        {
            var mock = new DiagnosticsMock();
            int pingIntervalMs = 5000;
            string nodeName = "Any";
            string groupName = "Any";
            var endTime = DateTime.Now.AddMilliseconds(5000);
            var startTime = DateTime.Now.AddMilliseconds(-5000);
            var dig = new Diagnostics(pingIntervalMs, mock);
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            mock.OnlinePeriods.Add(new OnlinePeriod { Id = 1, NodeId = 1, StartTime = startTime, EndTime = endTime });

            dig.HandlePing("Wrong", groupName);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException), "Such group exists")]
        public void HandlePingWhenWrongGroupNameTest()
        {
            var mock = new DiagnosticsMock();
            int pingIntervalMs = 5000;
            string nodeName = "Any";
            string groupName = "Any";
            var endTime = DateTime.Now.AddMilliseconds(5000);
            var startTime = DateTime.Now.AddMilliseconds(-5000);
            var dig = new Diagnostics(pingIntervalMs, mock);
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName, NodeGroupId = 1 });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            mock.OnlinePeriods.Add(new OnlinePeriod { Id = 1, NodeId = 1, StartTime = startTime, EndTime = endTime });

            dig.HandlePing(nodeName, "Wrong");
        }
    }
}