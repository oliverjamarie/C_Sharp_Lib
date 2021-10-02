namespace Library.Graph
{
    partial class Graph<T>
    {
        public class Edge
        {
            Node destNode;
            double cost;


            /// <summary>
            /// Edge constructor
            /// </summary>
            /// <param name="targetNode">Edge's destination node</param>
            /// <param name="cost">Cost to travel through the edge to the destination node</param>
            public Edge(Node targetNode, double cost)
            {
                this.destNode = targetNode;
                this.cost = cost;
            }


            /// <summary>
            /// Returns the edge's destination node
            /// </summary>
            /// <returns>
            /// Destination node
            /// </returns>
            public Node getDestNode()
            {
                return destNode;
            }


            /// <summary>
            /// Returns the cost to go the edge's destination node
            /// </summary>
            /// <returns>Cost to destination node</returns>
            public double getCost()
            {
                return cost;
            }
        }
    }
}
