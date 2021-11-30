using System;
using System.Collections.Generic;

namespace C_Sharp_Lib.Library.Graph
{
   
    partial class Graph<T>
    {
        protected class Edge : IEdge<T>
        {
            INode<T> destNode;
            double cost;


            /// <summary>
            /// Edge constructor
            /// </summary>
            /// <param name="targetNode">Edge's destination node</param>
            /// <param name="cost">Cost to travel through the edge to the destination node</param>
            public Edge(INode<T> targetNode, double cost)
            {
                this.destNode = targetNode;
                this.cost = cost;
            }


            /// <summary>
            /// Returns the edge's destination node
            /// </summary>
            /// <returns>
            /// Destination node
            /// </returns>
            public INode<T> getDestNode()
            {
                return destNode;
            }

            public void setCost(double cost)
            {
                this.cost = cost;
            }


            /// <summary>
            /// Returns the cost to go the edge's destination node
            /// </summary>
            /// <returns>Cost to destination node</returns>
            public double getCost()
            {
                return cost;
            }
        }
    }
    
}
