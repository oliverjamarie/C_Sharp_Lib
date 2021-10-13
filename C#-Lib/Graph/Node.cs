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
            private List<Edge> connections;

            /// <summary>
            /// Has the node been visited?
            /// </summary>
            public bool visited;

            /// <summary>
            /// Can a node connect to itself?
            /// </summary>
            private bool allowSelfConnection;

            public T data{ get; set; }

            public Node(T t)
            {
                connections = new List<Edge>();
                this.data = t;
                visited = false;
                allowSelfConnection = false;
            }

            public Node(T t, bool allowSelfConnection)
            {
                connections = new List<Edge>();
                this.data = t;
                visited = false;
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

                foreach (Edge e in connections)
                {
                    // checks if a connection to the target node already exists
                    if (e.getDestNode().Equals(this))
                        return false;
                }

                connections.Add(new Edge(destNode, cost));
                
                return true;
            }

            public Dictionary<T, double> getWeightedConnections()
            {
                Dictionary<T, double> dict = new Dictionary<T, double>();
                
                foreach(Edge edge in connections)
                {
                    dict.Add(edge.getDestNode().data, edge.getCost());
                }

                return dict;
            }

            /// <summary>
            /// Get list of connected nodes
            /// </summary>
            /// <returns>list of connected nodes </returns>
            public List<Node> getConnectedNodes()
            {
                List<Node> nodes = new List<Node>();
                foreach (Edge edge in connections)
                {
                    nodes.Add(edge.getDestNode());
                }

                return nodes;
            }

            public List<Node> getConnectedNodesSorted()
            {
                List<Node> unsorted = getConnectedNodes();
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
            public List<T> getConnectionsData()
            {
                List<T> list = new List<T>();
                List<Node> nodes = getConnectedNodes();

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
            public List<Edge> getConnections()
            {
                return connections;
            }

            /// <summary>
            /// Creates a sorted <code>List<Edge></code> based off the connection's cost
            /// </summary>
            /// <returns></returns>
            public List<Edge> getConnectionsSorted()
            {
                List<Edge> unsorted = new List<Edge>(connections);
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
            /// Compares this node with <paramref name="other"/>
            /// </summary>
            /// <param name="other">Node to compare to</param>
            /// <returns>TRUE if equal.  FALSE otherwise</returns>
            public bool Equals(Node other)
            {
                return data.Equals(other.data);
            }
            
            public bool updateConnection(Node dest, double cost)
            {
                foreach(Edge edge in connections)
                {
                    if (edge.getDestNode().Equals(dest))
                    {
                        edge.setCost(cost);

                        return true;
                    }
                }

                return false;
            }
        }
    }
}
