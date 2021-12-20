using System;
using Library.Graph;

namespace Library.Graph
{
    public interface IEdge<T> where T : IComparable
    {
        public INode<T> DestNode { get; }
        public INode<T> SourceNode { get; }
        public double Cost { get; set; }
    }
}
