using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Graph;


namespace Library.Graph.Algorithms
{
    public class PreferredAttachment <T> where T : IComparable
    {
        public Graph<T> graph;
        public INode<T> curr;

        double costModifier;

        public PreferredAttachment()
        {
            // Allow SelfConnections
            graph = new Graph<T>();
            init();
        }

        public PreferredAttachment(List<T> list)
        {
            graph = new Graph<T>(list);
            init();
        }

        public PreferredAttachment(Dictionary<T, List<T>> adjList)
        {
            graph = new Graph<T>(adjList);
            init();
        }

        public PreferredAttachment(Dictionary<T, Dictionary<T, double>> adjList)
        {
            graph = new Graph<T>(adjList);
            init();
        }

        public PreferredAttachment(Graph<T> graph)
        {
            this.graph = new Graph<T>(graph);
            init();
        }

        private void init()
        {
            graph.AllowSelfConnect = true;
            costModifier = 0.05;
            curr = graph.getRoot();
            balanceNode(curr);
        }

        public bool setCostModifier(double modifier)
        {
            if (modifier >= 0 && modifier <= 1.0)
            {
                costModifier = modifier + 1;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Traverses through graph generating a <see cref="Queue{T}"/>
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>

        public Queue<T> genQueue(int len)
        {
            Queue<T> queue = new Queue<T>();

            for (int i = 0; i < len; i++)
            {
                queue.Enqueue(getNext());
            }

            return queue;
            
        }


        /// <summary>
        /// <see cref="curr"/> travels to next <see cref="INode{T}"/>
        /// </summary>
        /// <returns>Data held in <see cref="INode{T}"/> <see cref="curr"/> travels to</returns>
        public T getNext()
        {
            // Need to specify System.Random as it the library is meant for Unity
            // and they have their own Random class
            System.Random random = new Random();
            double randNum = random.NextDouble(), cost = 0.0;
            List<IEdge<T>> sortedConnections;
            int index = 0, foundIndex = 0;

            if (curr == null)
                throw new PrefAttachNullRootException();

            #region Travel to next node
            sortedConnections = curr.getEdgesSorted();

            foreach(IEdge<T> edge in sortedConnections)
            {
                cost += edge.Cost;

                if (randNum >= cost)
                {
                    curr = edge.DestNode;
                    foundIndex = index;
                    break;
                }
                index++;
            }
            #endregion

            #region Update costs
            sortedConnections = curr.getEdgesSorted();

            for (int j = 0; j < sortedConnections.Count; j++)
            {
                cost = sortedConnections[j].Cost;

                if (j == foundIndex)
                {
                    cost *= (1.0 - costModifier);
                }
                else
                {
                    cost *= (1.0 + costModifier);
                }

                curr.updateCostToNeighbor(sortedConnections[j].DestNode, cost);
                
            }
            #endregion

            balanceNode(curr);
            return curr.Data;
        }

        /// <summary>
        /// Ensures that the cost of traveling to all of <see cref="node"/>'s is 1.0
        /// </summary>
        /// <param name="node"></param>
        public void balanceNode(INode<T> node)
        {
            List<IEdge<T>> edges = node.getEdgesSorted();
            double sum = 0.0;

            foreach (IEdge<T> edge in edges)
            {
                sum += edge.Cost;
            }

            if (sum - 1.0 < double.Epsilon)
                return;


            foreach (IEdge<T> edge in edges)
            {
                edge.Cost /= sum;
            }
        }

        public void tuneConnection(T source, T dest, bool increase)
        {
            if (graph.contains(source) == false && graph.contains(dest))
            {
                return;
            }

            System.Random random = new Random();
            double rand = random.NextDouble() * 0.001;

            if (increase)
                rand = 1 + rand;
            else
                rand = 1 - rand;

            graph.updateConnection(source, dest, rand);

            balanceNode(graph.GetNode(source));
        }
    }
}

