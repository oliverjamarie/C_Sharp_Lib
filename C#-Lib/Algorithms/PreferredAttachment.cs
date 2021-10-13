using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Graph;


namespace Library.Algorithms
{
    class PreferredAttachment <T> where T : IComparable
    {
        public Graph<T> graph;
        
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

        public PreferredAttachment(Graph<int> graph1)
        {
        }

        private void init()
        {
            graph.allowSelfConnect = true;
            costModifier = 0.05;
        }

        /// <summary>
        /// Generates a queue 
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>

        public Queue<T> genQueue(int len)
        {
            Queue<T> queue = new Queue<T>();
            Graph<T>.Node curr = graph.getRoot();
            List<Graph<T>.Edge> sortedConnections;        
            System.Random rnd = new System.Random();
            double randNum;
            int index, foundIndex;

            if (curr == null)
                return queue;

            for (int i = 0; i < len; i++)
            {
                index = 0;
                foundIndex = 0;
                randNum = rnd.NextDouble();
                sortedConnections = curr.getConnectionsSorted();

                foreach (Graph<T>.Edge edge in sortedConnections)
                {
                    if (randNum <= edge.getCost())
                    {
                        curr = edge.getDestNode();
                        queue.Enqueue(curr.data);
                        foundIndex = index;
                        break;
                    }
                    index++;
                }

                sortedConnections = curr.getConnectionsSorted();

                // TODO: update the graph costs
                double cost;

                for(int j = 0; j < sortedConnections.Count; j++)
                {
                    cost = sortedConnections[j].getCost();

                    if (j == foundIndex)
                    {
                        cost += costModifier;
                    }
                    else
                    {
                        cost -= costModifier;
                    }

                    curr.updateConnection(sortedConnections[j].getDestNode(), cost);
                }
            }

            return queue;
            
        }
    }
}
