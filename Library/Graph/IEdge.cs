using System;
using C_Sharp_Lib.Library.Graph;
namespace C_Sharp_Lib.Library.Graph
{
    public interface IEdge<T> where T : IComparable
    {
        public INode<T> getDestNode();
        public double getCost();
    }
}
