using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticClient
{
    class Program

    {
        static void Main()
        {
            MyModel context = new MyModel();
            Node node = new Node();
            NodeGroup nodegroup = new NodeGroup();
            
            context.SaveChanges();
            
        }
    }
}
