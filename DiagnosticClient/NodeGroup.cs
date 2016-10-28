using System.Collections.Generic;

namespace DiagnosticClient
{
    public class NodeGroup
    {
        public NodeGroup()
        {
            Nodes = new List<Node>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Node> Nodes { get; set; }
    }
}