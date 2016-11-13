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
            var context = new DiagnosticDbContext();
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

                float pingIntervalSec = EnterPing();
                Diagnostics diagnostics = new Diagnostics(pingIntervalSec);
                
                Console.WriteLine("Enter the name of nodegroup");
                string groupName = Console.ReadLine();

                try
                {
                    Node node;
                    if (string.IsNullOrWhiteSpace(groupName))
                        node = context.Nodes.Single(g => g.Name.ToUpper() == nodeName.ToUpper() && g.NodeGroupId == null);
                    else
                        node = context.Nodes.Single(g => g.Name.ToUpper() == nodeName.ToUpper() && g.Group.Name.ToUpper()==groupName.ToUpper()  );

                    diagnostics.HandlePing(nodeName, groupName);
                }

                catch (InvalidOperationException)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(groupName))
                            throw new ArgumentException();

                        var groupId = context.Groups.Single(g => g.Name.ToUpper() ==groupName.ToUpper()).Id;

                        Console.WriteLine("The GroupId is " + groupId);

                        context.Nodes.Add(new Node { Name = nodeName, NodeGroupId = groupId });
                        context.SaveChanges();
                        diagnostics.HandlePing(nodeName, groupName);
                    }
                    catch (ArgumentException)
                    {
                        context.Nodes.Add(new Node { Name = nodeName });
                        context.SaveChanges();
                        diagnostics.HandlePing(nodeName, groupName);
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("Created new group");
                        context.Groups.Add(new Group { Name = groupName });
                        context.SaveChanges();

                        var group = context.Groups
                         .Single(g => g.Name == groupName);

                        context.Nodes.Add(new Node { Name = nodeName, NodeGroupId = group.Id });
                        context.SaveChanges();
                        diagnostics.HandlePing(nodeName, groupName);
                    }
                }
                Console.WriteLine("Enter S, if you want to see DB");
                if (Console.ReadKey().Key == ConsoleKey.S)
                {
                    ReadNodes();
                    ReadNodeGroups();
                    ReadNodeOnlinePeriods();
                    Console.WriteLine(DateTime.Now);
                }
            }
        }

        public static void ReadNodes()
        {
            DiagnosticDbContext context = new DiagnosticDbContext();
            var list = context.Nodes.OrderBy(c => c.Id).ToList();
            Console.WriteLine("Nodes");

            foreach (var item in list)
                Console.WriteLine("{0} | Name:{1} | GroupId:{2}", item.Id, item.Name, item.NodeGroupId);
        }

        public static void ReadNodeGroups()
        {
            DiagnosticDbContext context = new DiagnosticDbContext();
            var list = context.Groups.OrderBy(c => c.Id).ToList();
            Console.WriteLine("NodeGroups");

            foreach (var item in list)
                Console.WriteLine("{0} | Name:{1}", item.Id, item.Name);
        }

        public static void ReadNodeOnlinePeriods()
        {
            DiagnosticDbContext context = new DiagnosticDbContext();
            var list = context.NodesOnlinePeriods.OrderBy(c => c.Id).ToList();
            Console.WriteLine("NodeOnlinePeriods");

            foreach (var item in list)
                Console.WriteLine("{0} | NodeId: {1} | Start:{2} | End:{3}", item.Id, item.NodeId, item.StartTime, item.EndTime);
        }

        public static float EnterPing()
        {
            string pingInterval;
            bool key = true;
            while (key)
            {
                try
                {
                    Console.WriteLine("Enter the interval of ping");
                    pingInterval = Console.ReadLine();
                    float pingIntervalMs = Convert.ToSingle(pingInterval);
                    key = false;
                    return pingIntervalMs;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Interval must be number");
                }
            }
            throw new ArgumentNullException();
        }
    }
}
