using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Sharp_Lib.Library.Graph;


namespace C_Sharp_Lib.Library.GraphLibrary.Graph.Algorithms
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
            graph.allowSelfConnect = true;
            costModifier = 0.05;
            curr = graph.getRoot();
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
        /// Generates a queue 
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

        public T getNext()
        {
            // Need to specify System.Random as it the library is meant for Unity
            // and they have their own Random class
            System.Random random = new Random();
            double randNum = random.NextDouble(), cumulCost = 0, cost;
            List<IEdge<T>> sortedConnections;
            int index = 0, foundIndex = 0;

            if (curr == null)
                return default; // discouraged for IComparable variables to be null

            sortedConnections = curr.getEdgesSorted();

            foreach(IEdge<T> edge in sortedConnections)
            {
                if (randNum <= cumulCost)
                {
                    curr = edge.getDestNode();
                    foundIndex = index;
                    break;
                }
                else
                {
                    cumulCost += edge.getCost();
                }

                index++;
            }

            sortedConnections = curr.getEdgesSorted();

            for (int j = 0; j < sortedConnections.Count; j++)
            {
                cost = sortedConnections[j].getCost();

                if (j == foundIndex)
                {
                    cost /= costModifier;
                }
                else
                {
                    cost *= costModifier;
                }

                curr.updateCostToNeighbor(sortedConnections[j].getDestNode(), cost);
            }

            return curr.getData();
        }
    }
}
