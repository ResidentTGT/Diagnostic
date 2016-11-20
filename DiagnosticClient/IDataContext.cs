using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticClient
{
    public interface IDataContext
    {
        void AddNodeOnlinePeriod(string nodeName, string groupName, DateTime startTime,DateTime endTime);
        bool IsNodeOnline(string nodeName, string groupName);
        void SetEndTime(string nodeName, string groupName, DateTime endTime);
        void AddLog(string nodeName, string groupName, string logLevel, string logMessage, DateTime logTime);
        bool CheckNodeExistence(string nodeName, string groupName);       
    }
}
