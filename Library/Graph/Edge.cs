using System;
using System.Collections.Generic;

namespace Library.Graph
{
   
    partial class Graph<T>
    {
        protected class Edge : IEdge<T>
        {
            INode<T> destNode, sourceNode;
            double cost;

            public INode<T> DestNode => destNode;

            public INode<T> SourceNode => sourceNode;

            public double Cost { get => cost; set => cost = value; }




            /// <summary>
            /// Edge constructor
            /// </summary>
            /// <param name="targetNode">Edge's destination node</param>
            /// <param name="cost">Cost to travel through the edge to the destination node</param>
            public Edge(INode<T> targetNode, INode<T> sourceNode, double cost)
            {
                this.destNode = targetNode;
                this.sourceNode = sourceNode;
                this.cost = cost;
            }

        }
    }
    
}
