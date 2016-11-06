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
            using (DiagnosticContext context = new DiagnosticContext())
            {

                while (Console.ReadKey().Key != ConsoleKey.Escape)
                {
                    string nodeName;
                    do
                    {
                        Console.WriteLine("\nEnter the name of node");
                        nodeName = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nodeName))
                            Console.WriteLine("Name cannot be empty");
                    }
                    while (string.IsNullOrWhiteSpace(nodeName));

                    float pingIntervalSec = Diagnostics.EnterPing();
                    Diagnostics diagnostics = new Diagnostics(pingIntervalSec);

                    try
                    {
                        int nodeId = DiagnosticContext.GetNodeId(nodeName);
                        DateTime startTime = DateTime.Now;
                        diagnostics.HandlePing(nodeName, startTime, pingIntervalSec);
                    }
                    catch (NullReferenceException)
                    {
                        DateTime startTime = DateTime.Now;
                        Console.WriteLine("Enter the name of nodegroup");
                        string nodeGroup = Console.ReadLine();

                        try
                        {
                            int groupId = DiagnosticContext.GetGroupId(nodeGroup);
                            Console.WriteLine("The GroupId is" + groupId);

                            context.Nodes.Add(new Node { Name = nodeName, NodeGroupId = groupId });
                            context.SaveChanges();
                            diagnostics.HandlePing(nodeName, startTime, pingIntervalSec);
                        }
                        catch (ArgumentException)
                        {
                            context.Nodes.Add(new Node { Name = nodeName });
                            context.SaveChanges();
                            diagnostics.HandlePing(nodeName, startTime, pingIntervalSec);
                        }
                        catch (NullReferenceException)
                        {
                            Console.WriteLine("Created new group");
                            context.NodeGroups.Add(new NodeGroup { Name = nodeGroup });
                            context.SaveChanges();

                            var group = context.NodeGroups
                             .Where(g => g.Name == nodeGroup)
                             .FirstOrDefault();

                            context.Nodes.Add(new Node { Name = nodeName, NodeGroupId = group.Id });
                            context.SaveChanges();
                            diagnostics.HandlePing(nodeName, startTime, pingIntervalSec);
                        }

                    }
                    Console.WriteLine("Нажмите S, если хотите вывести таблицы БД");
                    if (Console.ReadKey().Key == ConsoleKey.S)
                    {
                        DiagnosticContext.ReadNodes();
                        DiagnosticContext.ReadNodeGroups();
                        DiagnosticContext.ReadNodeOnlinePeriods();
                    }
                }
            }
        }
    }
}
