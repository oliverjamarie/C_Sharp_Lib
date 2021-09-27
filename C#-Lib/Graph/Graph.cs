using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Graph
{
    partial class Graph<T>
    {

        List<Node> nodes;

        public Graph()
        {
            nodes = new List<Node>();
        }

        private bool existsInGraph(T data)
        {
            foreach(Node node in nodes)
            {
                if (node.data.Equals(data))
                {
                    return true;
                }
            }

            return false; 
        }

        private Node Find(T data)
        {
            foreach (Node node in nodes)
            {
                if (node.data.Equals(data))
                {
                    return node;
                }
            }

            return null;
        }

        public bool insertNode(Node n)
        {
            if (nodes.Contains(n))
                return false;

            nodes.Add(n);

            return true;
        }

        public bool insert(T t)
        {
            return insertNode(new Node(t));
        }

        public bool connectNodes(T start, T dest, double cost)
        {
            Node startNode = Find(start), destNode = Find(dest);

            if (startNode != null && destNode != null)
                return connectNodes(startNode, destNode, cost);

            return false;
        }


        protected bool connectNodes(Node start, Node end, double cost)
        {
            if ((nodes.Contains(start) && nodes.Contains(end)) == false)
                return false;

            return start.connectNode(end, cost);
        }

        public void dispGraphBFT()
        {

            resetVisitedNodes();

            Queue<Node> queue = new Queue<Node>();

            queue.Enqueue(nodes[0]);

            while(queue.Count > 0)
            {
                Node node = queue.Dequeue();

                if (node.visited == false)
                    Console.Write($"{node.data}\t");

                List<Edge> connections = node.getConnections();

                if (node.getConnectedNodes().Count > 0)
                {
                    foreach (Edge edge in node.getConnections())
                    {
                        if (node.visited == false)
                            queue.Enqueue(edge.getDestNode());
                    }
                }

                node.visited = true;
            }

            Console.WriteLine("");
        }


        public void dispGraphDFT()
        {
            resetVisitedNodes();
            dispGraphDFT(nodes[0]);
            Console.WriteLine("");
        }

        private void dispGraphDFT(Node node)
        {
            Console.Write($"{node.data}\t");
            node.visited = true;

            foreach (Edge edge in node.getConnections())
            {
                Node n = edge.getDestNode();

                if (n.visited == false)
                {
                    dispGraphDFT(n);
                    
                }

                n.visited = true;
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
                List<Node> neighbors = nodes[row].getConnectedNodes();

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
        

        public Dictionary<T, List<T>> getAdjacencyList(){
            Dictionary<T, List<T>> dict = new Dictionary<T, List<T>>();

            foreach(Node node in nodes){
                dict.Add(node.data, node.getConnectionsData());
            }

            return dict;
        }

        public int getSize()
        {
            return nodes.Count;
        }
    }
}
