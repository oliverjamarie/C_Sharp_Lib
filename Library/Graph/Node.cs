using System;
using System.Collections.Generic;

namespace Library.Graph
{
    partial class Graph<T>
    {
        public class Node
        {
            /// <summary>
            /// Node's connections
            /// </summary>
            private List<Edge> neighbors;

            /// <summary>
            /// Has the node been visited?
            /// </summary>
            public bool visited;

            /// <summary>
            /// Can a node connect to itself?
            /// </summary>
            private bool allowSelfConnection;

            public T data{ get; set; }

            public static int countNodes = 0;
            public int id;

            public Node()
            {
                neighbors = new List<Edge>();
                allowSelfConnection = false;
                visited = false;
                id = countNodes++;
            }

            public Node(T t):this()
            {
                this.data = t;
            }

            public Node(T t, bool allowSelfConnection):this(t)
            {
                this.allowSelfConnection = allowSelfConnection;
            }

            /// <summary>
            /// Connects two nodes
            /// </summary>
            /// <param name="destNode">Destination node</param>
            /// <param name="cost">Cost to travel to node</param>
            /// <returns>
            /// FALSE if <code>allowSelfConnection</code> is FALSE && the node equals <paramref name="destNode"/>
            /// FALSE if a connection to <paramref name="destNode"/> already exists
            /// TRUE otherwise
            /// </returns>
            public bool connectNode(Node destNode, double cost)
            {
                if (this.Equals(destNode) && !allowSelfConnection)
                    return false;

                foreach (Edge e in neighbors)
                {
                    // checks if a connection to the target node already exists
                    if (e.getDestNode().Equals(this))
                        return false;
                }

                neighbors.Add(new Edge(destNode, cost));
                
                return true;
            }

            public Dictionary<T, double> getWeightedNeighbors()
            {
                Dictionary<T, double> dict = new Dictionary<T, double>();
                
                foreach(Edge edge in neighbors)
                {
                    dict.Add(edge.getDestNode().data, edge.getCost());
                }

                return dict;
            }

            /// <summary>
            /// Get list of connected nodes
            /// </summary>
            /// <returns>list of connected nodes </returns>
            public List<Node> getNeighbors()
            {
                List<Node> nodes = new List<Node>();
                foreach (Edge edge in neighbors)
                {
                    nodes.Add(edge.getDestNode());
                }

                return nodes;
            }

            public List<Node> getSortedNeighbors()
            {
                List<Node> unsorted = getNeighbors();
                List<Node> sorted = new List<Node>();
                T min = unsorted[0].data;
                
                while(unsorted.Count > 0)
                {
                    int minIndex = 0;

                    for (int i = 0; i < unsorted.Count; i++)
                    {
                        if (unsorted[i].data.CompareTo(min) <= 0)
                        {
                            minIndex = i;
                            min = unsorted[i].data;
                        }
                    }
                    sorted.Add(unsorted[minIndex]);
                    unsorted.RemoveAt(minIndex);
                }

                return sorted;
            }

            /// <summary>
            /// Gets a list of the datapoints connected to the node
            /// </summary>
            /// <returns>List<typeparamref name="T"/></returns>
            public List<T> getNeighborsData()
            {
                List<T> list = new List<T>();
                List<Node> nodes = getNeighbors();

                foreach (Node node in nodes)
                {
                    list.Add(node.data);
                }

                return list;
            }

            /// <summary>
            /// Get the edges the node is connected to
            /// </summary>
            /// <returns>List of edges connected to node</returns>
            public List<Edge> getEdges()
            {
                return neighbors;
            }

            /// <summary>
            /// Creates a sorted <code>List<Edge></code> based off the connection's cost
            /// </summary>
            /// <returns></returns>
            public List<Edge> getEdgesSorted()
            {
                List<Edge> unsorted = new List<Edge>(neighbors);
                List<Edge> sorted = new List<Edge>();
                int minIndex;

                while (unsorted.Count > 0)
                {
                    minIndex = 0;
                    Edge min = unsorted[minIndex];

                    for (int i = 0; i < unsorted.Count; i++)
                    {
                        if (unsorted[i].getCost() < unsorted[minIndex].getCost())
                        {
                            minIndex = i;
                            min = unsorted[i];
                        }
                    }
                    sorted.Add(min);
                    unsorted.RemoveAt(minIndex);
                }

                return sorted;
            }

            /// <summary>
            /// Compares this node with <paramref name="other"/> based on their IDs
            /// </summary>
            /// <param name="other">Node to compare to</param>
            /// <returns>TRUE if equal.  FALSE otherwise</returns>
            public bool Equals(Node other)
            {
                return id == other.id;
            }
            
            public bool updateCostToNeighbor(Node dest, double cost)
            {
                foreach(Edge edge in neighbors)
                {
                    if (edge.getDestNode().Equals(dest))
                    {
                        edge.setCost(cost);

                        return true;
                    }
                }

                return false;
            }

            private Node getConnectedNode(Node node)
            {
                foreach (Edge edge in neighbors)
                {
                    if (edge.getDestNode().Equals(node))
                    {
                        return edge.getDestNode();
                    }
                }

                return null;
            }

            public double getDistanceToNode(Node node)
            {
                foreach(Edge edge in neighbors)
                {
                    if (edge.getDestNode().Equals(node))
                    {
                        return edge.getCost();
                    }
                }
                

                return double.MinValue;
            }
        }
    }
}
