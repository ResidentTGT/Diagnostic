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
            using (MyModel context = new MyModel())
            {              
                while (Console.ReadKey().Key!=ConsoleKey.Escape)
                {
                    Console.WriteLine("Enter the name of node");
                    string node_name = Console.ReadLine();
                    DateTime time_start = DateTime.Now;
                   
                    if (node_name != "")
                    {                       
                        if (MyModel.GetNodeId(node_name) == 0)
                        {
                            Console.WriteLine("Enter the name of nodegroup");
                            string nodegroup = Console.ReadLine();

                            if (nodegroup == "")
                            {                              
                                context.Nodes.Add(new Node { Name = node_name });
                                context.SaveChanges();
                                Ping.HandlePing(node_name, time_start);                               
                            }
                            else
                            {

                                int id = MyModel.GetGroupId(nodegroup);
                                if (id == 0)
                                {

                                    context.NodeGroups.Add(new NodeGroup { Name = nodegroup });
                                    context.SaveChanges();

                                    var group = context.NodeGroups
                                     .Where(g => g.Name == nodegroup)
                                     .FirstOrDefault();

                                    context.Nodes.Add(new Node { Name = node_name, NodeGroupId = group.Id });
                                    context.SaveChanges();
                                    Ping.HandlePing(node_name, time_start);

                                }
                                else
                                {
                                    context.Nodes.Add(new Node { Name = node_name, NodeGroupId = id });
                                    context.SaveChanges();
                                    Ping.HandlePing(node_name, time_start);
                                }
                            }                                                    
                        }
                        else
                        {
                            Ping.HandlePing(node_name, time_start);
                        }
                    }
                    MyModel.ReadNodes();
                    MyModel.ReadNodeGroups();
                    MyModel.ReadNodeOnlinePeriods();
                }
            }
        }
    }
}
