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

        public PreferredAttachment(Graph<T> graph)
        {
            this.graph = new Graph<T>(graph);
            init();
        }

        private void init()
        {
            graph.allowSelfConnect = true;
            costModifier = 0.05;
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
            Graph<T> copyGraph = new Graph<T>(graph);
            Graph<T>.Node curr = copyGraph.getRoot();
            List<Graph<T>.Edge> sortedConnections;

            // Need to specify System.Random as it the library is meant for Unity
            // and they have their own Random class

            System.Random rnd = new System.Random();
            double randNum;
            int index, foundIndex;

            if (curr == null)
                return queue;

            while (queue.Count < len)
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
                    else
                    {
                        randNum -= edge.getCost();
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
                        cost /= costModifier;
                    }
                    else
                    {
                        cost *= costModifier;
                    }

                    curr.updateConnection(sortedConnections[j].getDestNode(), cost);
                }
            }

            return queue;
            
        }
    }
}
