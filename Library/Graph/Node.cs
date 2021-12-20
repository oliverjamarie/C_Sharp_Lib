using System;
using System.Collections.Generic;

namespace Library.Graph
{
    
    partial class Graph<T>
    {
        protected class Node : INode<T>
        {
            /// <summary>
            /// Node's connections
            /// </summary>
            private List<IEdge<T>> neighbors;

            /// <summary>
            /// Has the node been visited?
            /// </summary>
            private bool visited;

            /// <summary>
            /// Can a node connect to itself?
            /// </summary>
            private bool allowSelfConnection;

            private T data;

            protected static int countNodes = 0;
            protected int id;

            public bool Visited { get => visited; set => visited = value; }
            public bool AllowSelfConnection { get => allowSelfConnection; set => allowSelfConnection = value; }
            public T Data { get => data;}
            public int ID { get => id; }
            public static int NumNodes { get => countNodes; }

            public Node()
            {
                neighbors = new List<IEdge<T>>();
                AllowSelfConnection = false;
                Visited = false;
                id = countNodes++;
            }

            public Node(T t) : this()
            {
                this.data = t;
            }

            public Node(T t, bool allowSelfConnection) : this(t)
            {
                this.AllowSelfConnection = allowSelfConnection;
            }


            /// <summary>
            /// Connects two nodes
            /// </summary>
            /// <param name="destNode">Destination node</param>
            /// <param name="cost">Cost to travel to node</param>
            /// <returns>
            /// FALSE if <code>allowSelfConnection</code> is FALSE and the node equals <paramref name="destNode"/>
            /// FALSE if a connection to <paramref name="destNode"/> already exists
            /// TRUE otherwise
            /// </returns>
            public bool connectNode(INode<T> destNode, double cost)
            {
                if (this.Equals(destNode) && !AllowSelfConnection)
                    return false;

                foreach (Edge e in neighbors)
                {
                    // checks if a connection to the target node already exists
                    if (e.DestNode.Equals(this))
                        return false;
                }

                neighbors.Add(new Edge(destNode,this, cost));

                return true;
            }

            /// <summary>
            /// Compiles a dictionary of the other nodes this node is connected to
            /// and the cost to travel to them
            /// </summary>
            /// <returns></returns>
            public Dictionary<T, double> getWeightedNeighbors()
            {
                Dictionary<T, double> dict = new Dictionary<T, double>();

                foreach (Edge edge in neighbors)
                {
                    dict.Add(edge.DestNode.Data, edge.Cost);
                }

                return dict;
            }

            /// <summary>
            /// Get list of connected nodes
            /// </summary>
            /// <returns>list of connected nodes </returns>
            public List<INode<T>> getNeighbors()
            {
                List<INode<T>> nodes = new List<INode<T>>();
                foreach (Edge edge in neighbors)
                {
                    nodes.Add(edge.DestNode);
                }

                return nodes;
            }

            public List<INode<T>> getSortedNeighbors()
            {
                List<INode<T>> unsorted = getNeighbors();
                List<INode<T>> sorted = new List<INode<T>>();
                T min = unsorted[0].Data;

                while (unsorted.Count > 0)
                {
                    int minIndex = 0;

                    for (int i = 0; i < unsorted.Count; i++)
                    {
                        if (unsorted[i].Data.CompareTo(min) <= 0)
                        {
                            minIndex = i;
                            min = unsorted[i].Data;
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
                List<INode<T>> nodes = getNeighbors();

                foreach (Node node in nodes)
                {
                    list.Add(node.Data);
                }

                return list;
            }

            /// <summary>
            /// Get the edges the node is connected to
            /// </summary>
            /// <returns>List of edges connected to node</returns>
            public List<IEdge<T>> getEdges()
            {
                return neighbors;
            }

            /// <summary>
            /// Creates a sorted <code>List</code> based off the connection's cost
            /// </summary>
            /// <returns></returns>
            public List<IEdge<T>> getEdgesSorted()
            {
                List<IEdge<T>> unsorted = new List<IEdge<T>>(neighbors);
                List<IEdge<T>> sorted = new List<IEdge<T>>();
                int minIndex;

                while (unsorted.Count > 0)
                {
                    minIndex = 0;
                    IEdge<T> min = unsorted[minIndex];

                    for (int i = 0; i < unsorted.Count; i++)
                    {
                        if (unsorted[i].Cost < unsorted[minIndex].Cost)
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
            protected bool Equals(Node other)
            {
                return id == other.id;
            }

            public bool updateCostToNeighbor(INode<T> dest, double cost)
            {
                foreach (Edge edge in neighbors)
                {
                    if (edge.DestNode.Equals(dest))
                    {
                        edge.Cost = cost;

                        return true;
                    }
                }

                return false;
            }

            public bool incrementCostToNeighbor(INode<T> dest, double pct)
            {
                foreach (Edge edge in neighbors)
                {
                    if (edge.DestNode.Equals(dest))
                    {
                        double cost = edge.Cost;

                        edge.Cost = cost * pct;

                        return true;
                    }
                }

                return false;
            }

            public INode<T> getConnectedNode(INode<T> node)
            {
                foreach (Edge edge in neighbors)
                {
                    if (edge.DestNode.Equals(node))
                    {
                        return edge.DestNode;
                    }
                }

                return null;
            }

            public double getDistanceToNode(INode<T> node)
            {
                foreach (Edge edge in neighbors)
                {
                    if (edge.DestNode.Equals(node))
                    {
                        return edge.Cost;
                    }
                }


                return double.MinValue;
            }

            public bool Equals(INode<T> other)
            {
                if (other == null)
                    return false;

                return other.Data.CompareTo(data) == 0 && other.ID == ID;
            }
        }

    }
}
