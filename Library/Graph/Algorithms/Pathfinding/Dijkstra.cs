using System;
using System.Collections.Generic;
using Library.Graph;
using Library.Graph.Algorithms.Pathfinding;

namespace Library.Graph.Algorithms.Pathfinding
{
    public class Dijkstra<T> : Pathfinding<T> where T : IComparable
    {
        protected class Node : IComparable<Node>, IEquatable<Node>
        {
            public INode<T> node;
            public double totalDist;
            public Node prevNode;
            int id;
            static int countNodes = 0;

            public Node(INode<T> node, double totalDist, Node prevNode)
            {
                this.node = node;
                this.totalDist = totalDist;
                this.prevNode = prevNode;
                id = countNodes++;
            }

            public int CompareTo(Node other)
            {
                
                return totalDist.CompareTo(other.totalDist);
            }

            public bool Equals(Node other)
            {
                return id == other.id;
            }
        }


        List<Node> nodes;

        public Dijkstra(Graph<T> graph):base(graph)
        {
            nodes = new List<Node>();

            foreach(INode<T> node in graph.Nodes)
            {
                Console.WriteLine(node.ID);
                nodes.Add(new Node(node, double.PositiveInfinity, null));
            }
        }

        public override List<INode<T>> FindShortestPath(T start, T end)
        {
            return FindShortestPath(graph.GetNode(start), graph.GetNode(end));
        }

        public override List<INode<T>> FindShortestPath(INode<T> start, INode<T> end)
        {
            resetNodes();

            Node startNode = findNode(start);
            Node endNode = findNode(end);

            if (startNode == null || endNode == null)
                throw new Exception("Nodes not in graph");

            return prevNodes(FindShortestPath(startNode, endNode));
        }

        private Node FindShortestPath(Node curr, Node end)
        {
            List<Node> neighbors = findNodes(curr.node.getNeighbors());
            curr.node.Visited = true;

            foreach(Node node in neighbors)
            {
                node.prevNode = curr;
                node.totalDist = curr.node.getDistanceToNode(node.node);
            }

            Node lowest;

            while (HaveAllNodesBeenVisited() == false)
            {
                lowest = findClosestNode(nodes);
                lowest.node.Visited = true;

                neighbors = findNodes(lowest.node.getNeighbors());

                foreach (Node node in neighbors)
                {
                    if (node.node.Visited == false)
                    {
                        double tempDist = lowest.totalDist + lowest.node.getDistanceToNode(node.node);

                        if (tempDist < node.totalDist)
                        {
                            node.totalDist = tempDist;
                            node.prevNode = lowest;
                        }
                    }
                }
            }
            
            return end;
        }

        public override double ShortestDistance(INode<T> start, INode<T> end)
        {
            resetNodes();

            Node startNode = findNode(start);
            Node endNode = findNode(end);

            if (startNode == null || endNode == null)
                throw new Exception("Nodes not in graph");

            return FindShortestPath(startNode, endNode).totalDist;
        }

        public override double ShortestDistance(T start, T end)
        {
            return ShortestDistance(graph.GetNode(start), graph.GetNode(end));
        }


        private void resetNodes()
        {
            foreach(Node node in nodes)
            {
                node.node.Visited = false;
                node.prevNode = null;
                node.totalDist = double.PositiveInfinity;
            }
        }

        private Node findNode(INode<T> node)
        {
            
            foreach(Node n in nodes)
            {
                if (n.node.ID == node.ID)
                {
                    return n;
                }
            }

            throw new Exception("Node Not Found");
            return null;
        }

        private List<Node> findNodes(List<INode<T>> inodes)
        {
            List<Node> list = new List<Node>();

            foreach(INode<T> node in inodes)
            {
                list.Add(findNode(node));
            }

            return list;
        }

        private Node findClosestNode(List<Node> list)
        {
            Node minNode = null;
            double minDist = double.MaxValue;

            foreach(Node node in list)
            {
                if (node.node.Visited == false && minDist < node.totalDist)
                {
                    minNode = node;
                    minDist = node.totalDist;
                }
            }

            return minNode;
        }

        private List<INode<T>> prevNodes(Node node)
        {
            List<INode<T>> list = new List<INode<T>>();
            Node curr = node;
            int count = 0;

            while (curr.node != null)
            {
                list.Add(curr.node);
                curr = curr.prevNode;
            }

            list.Reverse();

            return list;
        }

        private bool HaveAllNodesBeenVisited()
        {
            foreach(Node node in nodes)
            {
                if (node.node.Visited == false)
                    return false;
            }

            return true;
        }

        
    }
}
