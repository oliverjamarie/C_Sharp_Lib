using System;
using System.Collections.Generic;
using C_Sharp_Lib.Library.Graph;

namespace C_Sharp_Lib.Library.Graph
{
    public interface INode<T> where T : IComparable
    {
        bool connectNode(INode<T> destNode, double cost);
        INode<T>getConnectedNode(INode<T>node);
        T getData();
        double getDistanceToNode(INode<T>node);
        List<IEdge<T>> getEdges();
        List<IEdge<T>> getEdgesSorted();
        int getID();
        List<INode<T>> getNeighbors();
        List<T> getNeighborsData();
        List<INode<T>> getSortedNeighbors();
        Dictionary<T, double> getWeightedNeighbors();
        bool incrementCostToNeighbor(INode<T>dest, double pct);
        bool isVisited();
        void setVisited(bool input);
        bool updateCostToNeighbor(INode<T>dest, double cost);
    }
}