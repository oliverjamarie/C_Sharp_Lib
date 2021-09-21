using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    class Graph<T>
    {
        public class Node
        {
            List<Edge> connections;
            public bool visited;


            T data;
            Node prevNode;
            double costToPrevNode;


            public Node(T t)
            {
                connections = new List<Edge>();
                this.data = t;
                visited = false;
                costToPrevNode = 0;
            }

            public T getData()
            {
                return data;
            }

            public bool setData(T data)
            {
                this.data = data;
                return true;
            }

            public bool connectNode(Node destNode, double cost)
            {
                if (this.Equals(destNode))
                    return false;

                foreach(Edge e in connections)
                {
                    if (e.getDestNode().data.Equals(destNode.data))
                        return false;
                }

                connections.Add(new Edge(destNode, cost));

                return true;
            }

            public void dispBFT()
            {
               foreach(Edge e in connections)
                {
                    
                }
            }

            void dispBFT(Node n)
            {
                
            }

        }

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

        List<Node> nodes;

        public Graph()
        {
            nodes = new List<Node>();
        }

        public bool insertNode(Node n)
        {
            if (nodes.Contains(n))
                return false;

            nodes.Add(n);

            return true;
        }

        public bool connectNodesUniDirection(Node start, Node end, double cost)
        {
            if ((nodes.Contains(start) && nodes.Contains(end)) == false)
                return false;


            return start.connectNode(end, cost);
        }

        public bool connectNodesBiDirection(Node start, Node end, double cost)
        {
            return connectNodesUniDirection(start, end, cost) && connectNodesUniDirection(end, start, cost);
        }

        public void displayGraphBFT()
        {
            foreach(Node n in nodes)
            {
                if (!n.visited)
                {
                    n.dispBFT();
                }
            }

        }

        public void resetVisitedNodes()
        {
            foreach (Node node in nodes)
            {
                node.visited = false;
            }
        }
    }
}
