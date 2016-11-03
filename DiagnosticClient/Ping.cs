using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DiagnosticClient
{
    
    public class Ping
    {
        static double EPS = 10 * 10E6;
        public Ping() { }
        
        static public void HandlePing(string node_name, DateTime time)
        {           
            MyModel context = new MyModel();

            int nodeid = MyModel.GetNodeId(node_name);
            int id = GetIdNodeOnline(time, nodeid);

            if (id == 0)
            {
                context.NodesOnlinePeriods.Add(new NodeOnlinePeriod { TimeStart = time, NodeId = nodeid, TimeEnd = time.AddSeconds(10) });
                context.SaveChanges();
            }
            else
            {
                var nop = context.NodesOnlinePeriods
                                 .Where(g => g.Id == id)
                                 .FirstOrDefault();
                nop.TimeEnd = time.AddSeconds(10);
                context.SaveChanges();
            }

        }
        static int GetIdNodeOnline(DateTime time, int nodeid)
        {
            MyModel context = new MyModel();           
            var list = context.NodesOnlinePeriods.ToList();
            foreach (var nop in list)
            {
                if ((nop.NodeId == nodeid) && (Math.Abs(nop.TimeEnd.Ticks - time.Ticks) < EPS))
                {
                    return nop.Id;
                }

            }
            return 0;

        }
    }
}
