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
        public float offlineTimeSec;

        public Diagnostics(float pingIntervalSec)
        {
            offlineTimeSec = 2 * pingIntervalSec;
        }

        public void HandlePing(string nodeName, DateTime startTime, float pingIntervalSec)
        {
            DiagnosticContext context = new DiagnosticContext();
            
            int nodeId = DiagnosticContext.GetNodeId(nodeName);
            try
            {
                int id = GetIdNodeOnline(startTime, nodeId);
                var nodeOnlinePeriod = context.NodesOnlinePeriods
                                 .Where(g => g.Id == id)
                                 .FirstOrDefault();
                nodeOnlinePeriod.EndTime = startTime.AddSeconds(offlineTimeSec);
                context.SaveChanges();

            }
            catch(NullReferenceException)
            {              
                context.NodesOnlinePeriods.Add(new NodeOnlinePeriod { StartTime = startTime, NodeId = nodeId, EndTime = startTime.AddSeconds(offlineTimeSec) });
                context.SaveChanges();
            }

        }

        int GetIdNodeOnline(DateTime startTime, int nodeId)
        {
            NodeOnlinePeriod result;
            using (var ctx = new DiagnosticContext())
                result = ctx.NodesOnlinePeriods.Where(g => g.NodeId == nodeId).OrderByDescending(g => g.Id).FirstOrDefault();

            if ((result == null) || (Math.Abs(result.EndTime.Ticks - startTime.Ticks) > offlineTimeSec * 10E6))
                throw new NullReferenceException();
            else
                return result.Id;
        }

        static public float EnterPing()
        {
            string pingInterval;
            bool key = true;
            while (key)
            {
                try
                {
                    Console.WriteLine("Enter the interval of ping");
                    pingInterval = Console.ReadLine();
                    float pingIntervalSec = Convert.ToSingle(pingInterval);
                    key = false;
                    return pingIntervalSec;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Interval must be number");

                }
            }
            throw new ArgumentNullException();
        }

        void SaveLog(string nodeName, string logLevel, string logMessage, DateTime logTime, DateTime currentLogTime)
        {
            var context = new DiagnosticContext();
            int nodeId = context.Nodes.Where(g => g.Name == nodeName).FirstOrDefault().Id;
            context.NodeLogs.Add(new NodeLog { NodeId = nodeId, Time = logTime, Level = logLevel, Message = logMessage });
            context.SaveChanges();
        }
    }
}
