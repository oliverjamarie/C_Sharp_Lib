using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Graph
{
    public partial class Graph<T>
    {

        List<Node> nodes;
        public bool allowSelfConnect { get; set; }

        public Graph()
        {
            nodes = new List<Node>();
            allowSelfConnect = false;
        }

        public Graph(bool allowSelfConnect)
        {
            this.allowSelfConnect = allowSelfConnect;
            nodes = new List<Node>();
        }

        public Graph(List<T> list)
        {
            nodes = new List<Node>();
            
            foreach(T i in list)
            {
                insert(i);
            }
        }

        // used to create a graph based off an adjacency list
        public Graph(Dictionary<T,List<T>> adjList)
        {
            nodes = new List<Node>();

            foreach(KeyValuePair<T, List<T>> pair in adjList)
            {
                if (existsInGraph(pair.Key) == false)
                {
                    insert(pair.Key);
                }

                foreach(T t in pair.Value)
                {
                    if (existsInGraph(t) == false)
                    {
                        insert(t);
                    }

                    connectNodes(pair.Key, t, 1);
                }
            }
        }

        // used to create a graph based off an adjacency list
        // (includes weights)
        public Graph(Dictionary<T, Dictionary<T,double>> adjList)
        {
            nodes = new List<Node>();

            foreach (KeyValuePair<T, Dictionary<T, double>> pair in adjList){
                if (existsInGraph(pair.Key) == false)
                {
                    insert(pair.Key);
                }

                foreach(KeyValuePair<T, double> pair2 in pair.Value)
                {
                    if (existsInGraph(pair2.Key) == false)
                    {
                        insert(pair2.Key);
                    }

                    connectNodes(pair.Key, pair2.Key, pair2.Value);
                }
            }
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

        public bool connectNodes(T start, Dictionary<T,double> targets)
        {
            foreach(KeyValuePair<T,double> pair in targets) {
                if (!connectNodes(start, pair.Key, pair.Value))
                {
                    return false;
                }
            }

            return true;
        }

        public bool connectNodes(T start, List<T> targets)
        {
            foreach(T t in targets)
            {
                if (!connectNodes(start, t, 1))
                {
                    return false;
                }
            }

            return true;
        }


        private bool connectNodes(Node start, Node end, double cost)
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
