using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Graph
{
    public partial class Graph<T> where T : IComparable
    {

        protected List<Node> nodes;
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
        /// Traverses through graph using Breadth First Traversal starting from
        /// graph's root 
        /// </summary>
        /// <returns>List<typeparamref name="T"/> in order of Breadth First Traversal</returns>
        public List<T> BFT()
        {
            List<T> list = new List<T>();
            resetVisitedNodes();

            Queue<Node> queue = new Queue<Node>();

            queue.Enqueue(nodes[0]);

            while(queue.Count > 0)
            {
                Node node = queue.Dequeue();

                if (node.visited == false)
                    list.Add(node.data);

                List<Edge> connections = node.getEdges();

                if (node.getNeighbors().Count > 0)
                {
                    foreach (Edge edge in node.getEdges())
                    {
                        if (node.visited == false)
                            queue.Enqueue(edge.getDestNode());
                    }
                }

                node.visited = true;
            }
            return list;
        }

        /// <summary>
        /// Traverses through graph using Depth First Traversal starting from
        /// the graph's root
        /// </summary>
        /// <returns></returns>
        public List<T> DFT()
        {
            List<T> list = new List<T>();
            resetVisitedNodes();
            DFT(nodes[0], list);
            return list;
        }

        /// <summary>
        /// Traverses through graph using Depth First Traversal starting from
        /// the specified <paramref name="source"/> node
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public List<T> DFT(Node source)
        {
            List<T> list = new List<T>();

            if (getNodeIndex(source) == -1)
                return null;

            resetVisitedNodes();
            DFT(source, list);
            return list;
        }

        /// <summary>
        /// Recursive helper methd for <see cref="DFT()"/>
        /// </summary>
        /// <param name="node">Current node</param>
        /// <param name="list">Nodes already traversed</param>
        private void DFT(Node node, List<T> list)
        {
            list.Add(node.data);
            node.visited = true;

            foreach (Edge edge in node.getEdges())
            {
                Node n = edge.getDestNode();

                if (n.visited == false)
                {
                    DFT(n, list);            
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
        /// <returns> Graph's adjacency matrix where: 1 (connection exists) and 0 (connection does not exist).</returns>
        public int[,] getAdjacencyMatrix()
        {
            int[,] matrix = new int[nodes.Count, nodes.Count];

            for (int row = 0; row < nodes.Count; row++)
            {
                List<Node> neighbors = nodes[row].getNeighbors();

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

        /// <summary>
        /// Generates weighted adjacency matrix 
        /// </summary>
        /// <returns></returns>
        public double [,] getWeightedAdjacencyMatrix()
        {
            double[,] matrix = new double[nodes.Count, nodes.Count];

            for (int row = 0; row < nodes.Count; row++)
            {
                List<Node> neighbors = nodes[row].getNeighbors();

                for (int col = 0; col < nodes.Count; col++)
                {
                    if (neighbors.Contains(nodes[col]))
                        matrix[row, col] = nodes[row].getDistanceToNode(nodes[col]);
                    else
                        matrix[row, col] = 0;
                }
            }

            return matrix;
        }
        
        /// <summary>
        /// Generates unweighted adjacency list
        /// </summary>
        /// <returns>Adjacency list as a Dictionary</returns>
        public Dictionary<T, List<T>> getAdjacencyList(){
            Dictionary<T, List<T>> dict = new Dictionary<T, List<T>>();

            foreach(Node node in nodes){
                dict.Add(node.data, node.getNeighborsData());
            }

            return dict;
        }

        /// <summary>
        /// Generates weighted adjacency list
        /// </summary>
        /// <returns>Adjacency list as Dictionary with the cost to travel to each Node</returns>
        public Dictionary<T, Dictionary<T, double>> getWeightedAdjacencyList()
        {
            Dictionary<T, Dictionary<T, double>> dict = new Dictionary<T, Dictionary<T, double>>();
            foreach(Node node in nodes)
            {
                dict.Add(node.data, node.getWeightedNeighbors());
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
            return source.updateCostToNeighbor(dest, cost);
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
        private int getNodeIndex(Node node)
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

        public double[] dijkstra(Node source)
        {
            double[] distances = new double[nodes.Count];
            double[,] graph;
            int index = getNodeIndex(source);

            if (index == -1)
            {
                return null;
            }

            for(int i = 0; i < nodes.Count; i++)
            {
                distances[i] = double.MaxValue;
            }

            graph = getWeightedAdjacencyMatrix();

            distances[index] = 0;

            resetVisitedNodes();

            source.visited = true;

            foreach(Node curr in nodes)
            {
                if (curr != source)
                {
                    int currNodeindex = minDistance(distances);

                    nodes[currNodeindex].visited = true;

                    for (int otherNodeIndex = 0; otherNodeIndex < nodes.Count; otherNodeIndex++)
                    {
                        if (updateDikstra(currNodeindex, otherNodeIndex, distances))
                        {
                            distances[otherNodeIndex] = distances[currNodeindex] + graph[index, otherNodeIndex];
                        }
                                
                    }
                }
            }

            return distances;
        }

        /// <summary>
        /// Helper method for Dijkstra to see if <paramref name="distances"/>[<paramref name="currNodeIndex"/>] needs to be updated
        /// </summary>
        /// <param name="currNodeIndex">Node Dijkstra is currently evalauating</param>
        /// <param name="targetNodeIndex">Node which <paramref name="currNodeIndex"/> is currently evaluating against</param>
        /// <param name="distances">Distances Dijkstra has and needs to evaluate</param>
        /// <returns></returns>
        private bool updateDikstra(int currNodeIndex, int targetNodeIndex, double[] distances)
        {
            double[,] graph = getWeightedAdjacencyMatrix();

            if (nodes[currNodeIndex].visited)
                return false;
            if (graph[currNodeIndex, targetNodeIndex] == 0)
                return false;
            if (distances[currNodeIndex] == double.MaxValue)
                return false;
            if (distances[currNodeIndex] > distances[currNodeIndex] + graph[currNodeIndex, targetNodeIndex])
                return false;

            return true;
        }

        /// <summary>
        /// Helper method for Dijkstra. Finds the smallest distance from <paramref name="distances"/>
        /// whose Node has not yet been visited
        /// </summary>
        /// <param name="distances">Distances Dijkstra has and needs to evaluate</param>
        /// <returns>Returns index of Node Dijkstra needs to evaluate against</returns>
        private int minDistance(double[] distances)
        {
            int index = 0, minIndex = -1 ;
            double min = double.MinValue;

            foreach (Node node in nodes)
            {
                if (node.visited == false)
                {
                    if (distances[index] <= min)
                    {
                        min = distances[index];
                        minIndex = index;
                    }
                }

                index++;
            }

            return minIndex;
        }

        
    }
}
