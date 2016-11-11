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
        private readonly float offlineTimeMs;
        private IDataContext _context;

        public Diagnostics(float pingIntervalMs, IDataContext context)
        {
            _context = context;
            offlineTimeMs = 2 * pingIntervalMs;
        }

        public Diagnostics(float pingIntervalMs) : this(pingIntervalMs, new DataContext())
        {
        }

        public void HandlePing(string nodeName, string groupName)
        {
            var ctx = _context;
            try
            {
                ctx.CheckNode(nodeName, groupName);

                if (ctx.IsNodeOnline(nodeName, groupName, offlineTimeMs))
                    ctx.RewriteNodeOnlinePeriod(nodeName, groupName, offlineTimeMs);
                else
                    ctx.AddNodeOnlinePeriod(nodeName, groupName, offlineTimeMs);
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine("Проверка имен не пройдена, пингование не может быть осуществлено");
            }
        }

        public void SaveLog(string nodeName, string groupName, string logLevel, string logMessage, DateTime logTime)
        {
            var ctx = _context;
            try
            {
                ctx.CheckLog(nodeName, groupName, logLevel, logMessage, logTime);
                ctx.AddLog(nodeName, groupName, logLevel, logMessage, logTime);
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine("Проверка лога не пройдена, логирование не может быть осуществлено");
            }
        }
    }
}
