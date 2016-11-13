using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticClient
{
    public interface IDataContext
    {
        void AddNodeOnlinePeriod(string nodeName, string groupName, float offlineTimeMs);
        bool IsNodeOnline(string nodeName, string groupName, float offlineTimeMs);
        void RewriteNodeOnlinePeriod(string nodeName, string groupName, float offlineTimeMs);
        void AddLog(string nodeName, string groupName, string logLevel, string logMessage, DateTime logTime);
        bool CheckNodeExistence(string nodeName, string groupName);
        
    }
}
