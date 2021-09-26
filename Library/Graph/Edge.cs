namespace Library.Graph
{
    partial class Graph<T>
    {
        public class Edge
        {
            Node destNode;
            double cost;

            public Edge(Node targetNode, double cost)
            {
                this.destNode = targetNode;
                this.cost = cost;
            }

            public Node getDestNode()
            {
                return destNode;
            }

            public double getCost()
            {
                return cost;
            }
        }
    }
}
