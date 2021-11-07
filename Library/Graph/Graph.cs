using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Graph
{
    public partial class Graph<T> where T : IComparable
    {

        List<Node> nodes;
        public bool allowSelfConnect { get; set; }

        public Graph()
        {
            nodes = new List<Node>();
            allowSelfConnect = true;
        }

        public Graph(bool allowSelfConnect)
        {
            this.allowSelfConnect = allowSelfConnect;
            nodes = new List<Node>();
        }

        public Graph(List<T> list):this()
        {
            nodes = new List<Node>();
            allowSelfConnect = true;
            foreach(T i in list)
            {
                insert(i);
            }
           
        }

        public Graph(Graph<T> graph):this(graph.getWeightedAdjacencyList())
        {
            allowSelfConnect = true;
        }


        /// <summary>
        /// Constructor using an adjacency list
        /// </summary>
        /// <param name="adjList"> Key: source node; Value: list of destination nodes </param>
        public Graph(Dictionary<T,List<T>> adjList)
        {
            allowSelfConnect = true;
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

        /// <summary>
        /// Constructor using an adjacency list (includes weights to destination nodes)
        /// </summary>
        /// <param name="adjList">
        /// <list type="table">
        ///     <item><term>Key</term><description>Source node</description></item>
        ///     <item><term>Value</term><description>Dictionary containing the target nodes and the weight of their edges</description></item>
        /// </list>
        /// </param>
        public Graph(Dictionary<T, Dictionary<T,double>> adjList):this()
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

        /// <summary> 
        /// Checks if a node exists in  <code>nodes</code> based on the data a node containts.
        /// Dependent on <code>Find(T)</code> method
        /// </summary>
        /// <param name="nodeData">Node you're looking for </param>
        /// <returns>TRUE if it exists in nodes, FALSE otherwise</returns>
        public bool existsInGraph(T nodeData)
        {
            foreach(Node node in nodes)
            {
                if (node.data.Equals(nodeData))
                {
                    return true;
                }
            }

            return false; 
        }

        /// <summary>
        /// Finds node having <code>data</code> as <code>Node.data</code>
        /// </summary>
        /// <param name="data"></param>
        /// <returns>A Node from <code>nodes</code> having <code>data</code> as <code>Node.data</code>. Returns NULL if not found</returns>
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

        /// <summary>
        /// Inserts node  in <code>nodes</code> 
        /// </summary>
        /// <param name="n">Node n</param>
        /// <returns>TRUE if <code>nodes</code> does not contain <code>n</code>, FALSE otherwise</returns>
        private bool insertNode(Node n)
        {
            if (nodes.Contains(n))
                return false;

            nodes.Add(n);
            
            return true;
        }


        /// <summary>
        /// Inserts new node into <code>nodes</code>.  Dependent on <code>Insert(Node)</code> method
        /// </summary>
        /// <param name="t">What to add to the list of <code>nodes</code></param>
        /// <returns>Return value is <code>insert(Node)</code></returns>
        public bool insert(T t)
        {
            return insertNode(new Node(t));
        }


        /// <summary>
        /// Connects two nodes
        /// </summary>
        /// <param name="start"> Start Node </param>
        /// <param name="dest"> Destination Node</param>
        /// <param name="cost"> Cost to travel from <code>start</code> to <code>dest</code></param>
        /// <returns></returns>
        public bool connectNodes(T start, T dest, double cost)
        {
            Node startNode = Find(start), destNode = Find(dest);

            if (startNode != null && destNode != null)
                return connectNodes(startNode, destNode, cost);

            return false;
        }

        /// <summary>
        /// Connects <paramref name="start"/> to all nodes in <paramref name="targets"/> keeping track of their cost
        /// </summary>
        /// <param name="start">Start node</param>
        /// <param name="targets">Dictionary of target nodes and their weights</param>
        /// <returns></returns>
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

        /// <summary>
        /// Connects <paramref name="start"/> to all nodes in <paramref name="targets"/>
        /// </summary>
        /// <param name="start">Start Node</param>
        /// <param name="targets">List of target nodes</param>
        /// <returns></returns>
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

        /// <summary>
        /// Connects two Node objects 
        /// </summary>
        /// <param name="start">Start Node</param>
        /// <param name="end">Target Node</param>
        /// <param name="cost">Cost for <paramref name="start"/> to travel to <paramref name="end"/></param>
        /// <returns>TRUE if <code>nodes</code>does not contain <paramref name="start"/> or <paramref name="end"/></returns>
        private bool connectNodes(Node start, Node end, double cost)
        {
            if ((nodes.Contains(start) && nodes.Contains(end)) == false)
                return false;

            return start.connectNode(end, cost);
        }


        /// <summary>
        /// Displays graph using Breadth First Traversal
        /// </summary>
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

        /// <summary>
        /// Displays graph using Depth First Traversal
        /// </summary>
        public void dispGraphDFT()
        {
            resetVisitedNodes();
            dispGraphDFT(nodes[0]);
            Console.WriteLine("");
        }

        /// <summary>
        /// Displays graph using Depth First Traversal starting from <code>node</code>
        /// </summary>
        /// <param name="node">Root Node</param>
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

        /// <summary>
        /// Resets all nodes in <code>nodes</code> list not visited
        /// </summary>
        public void resetVisitedNodes()
        {
            foreach (Node node in nodes)
            {
                node.visited = false;
            }
        }

        /// <summary>
        /// Generates unweighted adjacency matirx.
        /// </summary>
        /// <returns>A square matrix of size <code>nodes.Count</code> of 1s (connection exists) and 0s (connection does not exist).</returns>
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

        public double [,] getWeightedAdjacencyMatrix()
        {
            double[,] matrix = new double[nodes.Count, nodes.Count];

            for (int row = 0; row < nodes.Count; row++)
            {
                List<Node> neighbors = nodes[row].getConnectedNodes();

                for (int col = 0; col < nodes.Count; col++)
                {
                    if (neighbors.Contains(nodes[col]))
                        matrix[row, col] = nodes[row].distToNode(nodes[col]);
                    else
                        matrix[row, col] = 0;
                }
            }

            return matrix;
        }
        
        /// <summary>
        /// Generates adjacency list
        /// </summary>
        /// <returns>Adjacency list as a Dictionary</returns>
        public Dictionary<T, List<T>> getAdjacencyList(){
            Dictionary<T, List<T>> dict = new Dictionary<T, List<T>>();

            foreach(Node node in nodes){
                dict.Add(node.data, node.getConnectionsData());
            }

            return dict;
        }

        public Dictionary<T, Dictionary<T, double>> getWeightedAdjacencyList()
        {
            Dictionary<T, Dictionary<T, double>> dict = new Dictionary<T, Dictionary<T, double>>();
            foreach(Node node in nodes)
            {
                dict.Add(node.data, node.getWeightedConnections());
            }
            return dict;
        }

        /// <summary>
        /// Gets the size of <code>nodes.Count</code>
        /// </summary>
        /// <returns>Number of nodes in graph</returns>
        public int getSize()
        {
            return nodes.Count;
        }

        private bool updateConnection(Node source, Node dest, double cost)
        {
            return source.updateConnection(dest, cost);
        }

        public bool updateConnection(T source, T dest, double cost)
        {
            Node sourceNode = Find(source);
            Node destNode = Find(dest);

            if (sourceNode == null || destNode == null)
            {
                return false;
            }

            return updateConnection(sourceNode, destNode, cost); 
        }

        public Node getRoot()
        {
            return nodes[0];
        }

        /// <summary>
        /// Goes through the graph's list of nodes and gets the data stored in every one
        /// </summary>
        /// <returns>Returns list of every piece of data in the graph</returns>
        public List<T> getNodes()
        {
            List<T> list = new List<T>();

            foreach(Node node in nodes)
            {
                list.Add(node.data);
            }

            return list;
        }

        /// <summary>
        /// Returns index of <paramref name="node"/> in nodes
        /// </summary>
        /// <param name="node"></param>
        /// <returns>-1 if not in nodes.
        /// Index of <paramref name="node"/> in nodes</returns>
        private int index(Node node)
        {
            int index = 0;

            foreach(Node n in nodes)
            {
                if (node.Equals(n))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }
    }
}
