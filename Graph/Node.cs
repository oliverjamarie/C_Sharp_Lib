using System;
using System.Collections.Generic;

namespace Library.Graph
{
    partial class Graph<T>
    {
        public class Node
        {
            private List<Edge> connections;

            public bool visited;
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

            //public T getData()
            //{
            //    return data;
            //}

            //public bool setData(T data)
            //{
            //    this.data = data;
            //    return true;
            //}

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

            public List<Node> getConnectedNodes()
            {
                List<Node> nodes = new List<Node>();
                foreach (Edge edge in connections)
                {
                    nodes.Add(edge.getDestNode());
                }

                return nodes;
            }

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

            public List<Edge> getConnections()
            {
                return connections;
            }

            public bool Equals(Node other)
            {
                return data.Equals(other.data);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Node);
            }
        }
    }
}
