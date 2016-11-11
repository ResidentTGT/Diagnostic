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
        public void HandlePingTest()
        {

            var mock = new DiagnosticsMock();

            float pingIntervalMs = 5000;
            int subFromRealTimeMs = 50000;
            string nodeName = "Any";
            string groupName = "Any";

            var time = new TimeSpan(0, 0, 0, 0, subFromRealTimeMs);
            var startTime = DateTime.Now;
            var endTime = startTime.Subtract(time);
            var dig = new Diagnostics(pingIntervalMs, mock);
            mock.Nodes.Add(new Node { Id = 1, Name = nodeName });
            mock.Groups.Add(new Group { Id = 1, Name = groupName });
            mock.OnlinePeriods.Add(new OnlinePeriod { Id = 1, NodeId = 1, StartTime = startTime, EndTime = endTime });
            dig.HandlePing(mock.Nodes[0].Name, mock.Groups[0].Name);

            Assert.AreEqual(mock.OnlinePeriods.Count, 2);
        }

        [TestMethod()]
        public void SaveLogTest()
        {
            var mock = new DiagnosticsMock();

            float pingIntervalMs = 5000;
            string nodeName = "Any";
            string groupName = "Any";
            string logLevel = "Any";
            string logMessage = "Any";
            DateTime logTime = DateTime.Now;

            var dig = new Diagnostics(pingIntervalMs, mock);
            dig.SaveLog(nodeName, groupName, logLevel, logMessage, logTime);

            Assert.AreEqual(mock.Logs.Count, 1);
            Assert.AreEqual(mock.Logs[0].Time, logTime);
        }
    }
}