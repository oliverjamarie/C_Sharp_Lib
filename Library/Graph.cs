using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    class Graph<T>
    {
        public class Node
        {
            public List<Edge> connections;
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

            public List<Node> getConnections()
            {
                List<Node> nodes = new List<Node>();

                foreach(Edge edge in connections)
                {
                    nodes.Add(edge.getDestNode());
                }

                return nodes;
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
            return connectNodesUniDirection(start, end, cost) &&
                connectNodesUniDirection(end, start, cost);
        }

        public void dispGraphBFT()
        {
            Queue<Node> queue = new Queue<Node>();

            queue.Enqueue(nodes[0]);

            while(queue.Count > 0)
            {
                Node node = queue.Dequeue();

                Console.WriteLine(node.getData());

                if (node.connections.Count > 0)
                {            
                    foreach (Edge edge in node.connections)
                    {
                        queue.Enqueue(edge.getDestNode());
                    }
                }

                node.visited = true;
            }

            Console.WriteLine("\n\n Method 2 \n");

            foreach(Node n in queue)
            {
                Node node = queue.Dequeue();

                Console.WriteLine(node.getData());

                if (node.connections.Count > 0)
                {
                    foreach (Edge edge in node.connections)
                    {
                        queue.Enqueue(edge.getDestNode());
                    }
                }

                node.visited = true;
            }
        }


        public void dispGraphDFT()
        {
            resetVisitedNodes();

            dispGrapDFT(nodes[0]);
        }

        private void dispGrapDFT(Node node)
        {
            foreach(Edge edge in node.connections)
            {
                Node n = edge.getDestNode();
                dispGrapDFT(n);
                n.visited = true;
                Console.WriteLine(n.getData());
            }
        }

        public void resetVisitedNodes()
        {
            foreach (Node node in nodes)
            {
                node.visited = false;
            }
        }

        public int[,] getAdjacencyMatrix()
        {
            int[,] matrix = new int[nodes.Count, nodes.Count];

            for (int row = 0; row < nodes.Count; row++)
            {
                List<Node> neighbors = nodes[row].getConnections();

                for (int col = 0; col < nodes.Count; col++)
                {
                    if (neighbors.Contains(nodes[col]))
                        matrix[row, col] = 1;
                    else
                        matrix[row, col] = 0;
                }
            }

            return matrix;
        }

        public LinkedList<Node> getAdjacencyList()
        {
            LinkedList<Node> listNodes = new LinkedList<Node>();

            foreach (Node node in nodes)
            {
                
            }

            return listNodes;
        }
    }
}
