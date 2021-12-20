using System;
using System.Collections.Generic;

namespace Library.Graph.Algorithms.Pathfinding
{
    public abstract class Pathfinding<T> where T : IComparable
    {
        protected Graph<T> graph;

        public Pathfinding(Graph<T> graph)
        {
            this.graph = new Graph<T>();
        }

        public abstract List<INode<T>> FindShortestPath(INode<T> start, INode<T> end);
        public abstract List<INode<T>> FindShortestPath(T start, T end);

        public abstract double ShortestDistance(INode<T> start, INode<T> end);
        public abstract double ShortestDistance(T start, T end);
    }
}
