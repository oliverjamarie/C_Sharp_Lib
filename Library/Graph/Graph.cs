using System;
using System.Collections.Generic;
using System.Text;
using Library.Graph.Algorithms;

namespace Library.Graph
{
    public partial class Graph<T> where T : IComparable
    {

        protected List<Node> nodes;
        private bool allowSelfConnect;

        public bool AllowSelfConnect
        {
            get { return allowSelfConnect; }
            set { allowSelfConnect = value; }
        }

        public int Size { get => nodes.Count; }
        public INode<T> Root { get => nodes[0]; }
        public List<INode<T>> Nodes
        {
            get
            {
                List<INode<T>> list = new List<INode<T>>();

                foreach(Node node in nodes)
                {
                    list.Add(node);
                }

                return list;
            }
        }

        #region Constructors
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
                if (contains(pair.Key) == false)
                {
                    insert(pair.Key);
                }

                foreach(T t in pair.Value)
                {
                    if (contains(t) == false)
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
                if (contains(pair.Key) == false)
                {
                    insert(pair.Key);
                }

                foreach(KeyValuePair<T, double> pair2 in pair.Value)
                {
                    if (contains(pair2.Key) == false)
                    {
                        insert(pair2.Key);
                    }

                    if (connectNodes(pair.Key, pair2.Key, pair2.Value) == false)
                        Console.WriteLine("Failed To Connect");
                }
            }
        }

        #endregion
        /// <summary>
        /// Checks if a node exists in  <code>nodes</code> based on the data a node containts.
        /// Dependent on <code>Find(T)</code> method
        /// </summary>
        /// <param name="nodeData">Node you're looking for </param>
        /// <returns>TRUE if it exists in nodes, FALSE otherwise</returns>
        public bool contains(T nodeData)
        {
            foreach(Node node in nodes)
            {
                if (node.Data.Equals(nodeData))
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
        protected Node Find(T data)
        {
            foreach (Node node in nodes)
            {
                if (node.Data.CompareTo(data) == 0)
                {
                    return node;
                }
            }

            throw new Exception("Node not in graph");
            return null;
        }

        public INode<T> GetNode(T data)
        {
            return Find(data);
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

        #region Connect Nodes
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
        #endregion Connect Nodes

        #region Traversal
        /// <summary>
        /// Traverses through graph using Breadth First Traversal starting from
        /// graph's root
        /// </summary>
        /// <returns>List<typeparamref name="T"/> in order of Breadth First Traversal</returns>
        public List<T> BFT()
        {
            List<T> list = new List<T>();
            resetVisitedNodes();

            Queue<INode<T>> queue = new Queue<INode<T>>();

            queue.Enqueue(nodes[0]);

            while(queue.Count > 0)
            {
                INode<T> node = queue.Dequeue();

                if (node.Visited == false)
                    list.Add(node.Data);

                List<IEdge<T>> connections = node.getEdges();

                if (node.getNeighbors().Count > 0)
                {
                    foreach (IEdge<T> edge in node.getEdges())
                    {
                        if (node.Visited == false)
                            queue.Enqueue(edge.DestNode);
                    }
                }

                node.Visited = true;
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
        protected List<T> DFT(Node source)
        {
            List<T> list = new List<T>();

            if (getNodeIndex(source) == -1)
                return null;

            resetVisitedNodes();
            DFT(source, list);
            return list;
        }

        public List<T> DFT(T data)
        {
            return DFT(Find(data));
        }

        /// <summary>
        /// Recursive helper methd for <see cref="DFT()"/>
        /// </summary>
        /// <param name="node">Current node</param>
        /// <param name="list">Nodes already traversed</param>
        private void DFT(INode<T> node, List<T> list)
        {
            list.Add(node.Data);
            node.Visited = true;

            foreach (Edge edge in node.getEdges())
            {
                INode<T> n = edge.DestNode;

                if (n.Visited == false)
                {
                    DFT(n, list);
                }

                n.Visited = true;
            }
        }
        #endregion Traversal

        /// <summary>
        /// Resets all nodes in <code>nodes</code> list not visited
        /// </summary>
        public void resetVisitedNodes()
        {
            foreach (INode<T> node in nodes)
            {
                node.Visited = false;
            }
        }

        #region Adjacency Matrices And Lists
        /// <summary>
        /// Generates unweighted adjacency matirx.
        /// </summary>
        /// <returns> Graph's adjacency matrix where: 1 (connection exists) and 0 (connection does not exist).</returns>
        public int[,] getAdjacencyMatrix()
        {
            int[,] matrix = new int[nodes.Count, nodes.Count];

            for (int row = 0; row < nodes.Count; row++)
            {
                List<INode<T>> neighbors = nodes[row].getNeighbors();

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
                List<INode<T>> neighbors = nodes[row].getNeighbors();

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
                dict.Add(node.Data, node.getNeighborsData());
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
                dict.Add(node.Data, node.getWeightedNeighbors());
            }
            return dict;
        }

        #endregion

        #region Change Cost To Travel To Node
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

        
        public bool incrementConnection(T source, T dest, double pct)
        {
            Node sourceNode = Find(source);
            Node destNode = Find(dest);

            if (sourceNode == null || destNode == null)
            {
                return false;
            }

            return incrementConnection(sourceNode, destNode, pct);
        }

        private bool incrementConnection(Node source, Node dest, double pct)
        {
            return source.incrementCostToNeighbor(dest, pct);
        }
        #endregion Change Cost To Travel To Node

        public INode<T> getRoot()
        {
            return nodes[0];
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

        //#region Dijkstra

        //public double[] dijkstra(T data)
        //{
        //    Node node = Find(data);

        //    return dijkstra(node);
        //}

        //public double minDistanceToDest(T source, T target)
        //{
        //    Node sourceNode = Find(source);
        //    Node destNode = Find(target);

        //    if (sourceNode == null || destNode == null)
        //        throw new ElementNotInGraphException();

        //    double[] distances = dijkstra(sourceNode);

        //    if (getNodeIndex(destNode) != -1)
        //    {
        //        return distances[getNodeIndex(destNode)];
        //    }

        //    throw new ElementNotInGraphException();
        //}

        //private double[] dijkstra(Node source)
        //{
        //    double[] distances = new double[nodes.Count];
        //    double[,] graph;
        //    int index = getNodeIndex(source);

        //    if (index == -1)
        //    {
        //        return null;
        //    }

        //    for(int i = 0; i < nodes.Count; i++)
        //    {
        //        distances[i] = double.MaxValue;
        //    }

        //    graph = getWeightedAdjacencyMatrix();

        //    distances[index] = 0;

        //    resetVisitedNodes();

        //    source.Visited =true;

        //    foreach (Node curr in nodes)
        //    {
        //        if (curr != source)
        //        {
        //            int currNodeindex = minDistance(distances);

        //            if(currNodeindex == -1)
        //            {
        //                continue;
        //            }

        //            nodes[currNodeindex].Visited = true;

        //            for (int otherNodeIndex = 0; otherNodeIndex < nodes.Count; otherNodeIndex++)
        //            {
        //                if (updateDikstra(currNodeindex, otherNodeIndex, distances))
        //                {
        //                    distances[otherNodeIndex] = distances[currNodeindex] + graph[index, otherNodeIndex];
        //                }
        //            }
        //        }
        //    }

        //    return distances;
        //}

        ///// <summary>
        ///// Helper method for <see cref="dijkstra(Node)"/> to see if
        ///// <paramref name="distances"/>[<paramref name="currNodeIndex"/>] needs to be updated
        ///// </summary>
        ///// <param name="currNodeIndex">Node Dijkstra is currently evalauating</param>
        ///// <param name="targetNodeIndex">Node which <paramref name="currNodeIndex"/> is currently evaluating against</param>
        ///// <param name="distances">Distances Dijkstra has and needs to evaluate</param>
        ///// <returns></returns>
        //private bool updateDikstra(int currNodeIndex, int targetNodeIndex, double[] distances)
        //{
        //    double[,] graph = getWeightedAdjacencyMatrix();

        //    if (nodes[currNodeIndex].Visited)
        //        return false;
        //    if (graph[currNodeIndex, targetNodeIndex] == 0)
        //        return false;
        //    if (distances[currNodeIndex] == double.MaxValue)
        //        return false;
        //    if (distances[currNodeIndex] > distances[currNodeIndex] + graph[currNodeIndex, targetNodeIndex])
        //        return false;

        //    return true;
        //}

        ///// <summary>
        ///// Helper method for <see cref="dijkstra(Node)"/>. Finds the smallest distance from <paramref name="distances"/>
        ///// whose Node has not yet been visited
        ///// </summary>
        ///// <param name="distances">Distances Dijkstra has and needs to evaluate</param>
        ///// <returns>Returns index of Node Dijkstra needs to evaluate against</returns>
        //private int minDistance(double[] distances)
        //{
        //    int index = 0, minIndex = -1 ;
        //    double min = double.MinValue;

        //    foreach (Node node in nodes)
        //    {
        //        if (node.Visited == false)
        //        {
        //            if (distances[index] <= min)
        //            {
        //                min = distances[index];
        //                minIndex = index;
        //            }
        //        }

        //        index++;
        //    }

        //    return minIndex;
        //}

        //#endregion Dijkstra
    }
}
