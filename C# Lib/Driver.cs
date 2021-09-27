using System;
using Library.Graph;
using System.Collections;
using System.Collections.Generic;

namespace Library
{
    public static class Driver
    {
        public static void Main()
        {
            testGraph2();
        }

        private static void testGraph1()
        {
            Graph<int> graph = new Graph<int>();

            graph.insert(0);

            for (int i = 1; i <= 5; i++)
            {
                graph.insert(i);
                graph.connectNodes(0, i, 1);
                graph.connectNodes(i, 0, 1);
            }

            traversal(graph);
            adjacenyList(graph);
            adjacenyList(graph);
        }


        private static void testGraph2()
        {
            Graph<int> graph = new Graph<int>();

            for (int i = 0; i <= 5; i++)
            {
                graph.insert(i);
            }

            graph.connectNodes(0, 2, 1);
            graph.connectNodes(0, 5, 1);

            graph.connectNodes(1, 2, 1);
            graph.connectNodes(1, 5, 1);

            graph.connectNodes(2, 1, 1);
            graph.connectNodes(2, 3, 1);

            graph.connectNodes(3, 0, 1);
            graph.connectNodes(3, 4, 1);

            graph.connectNodes(4, 0, 1);

            graph.connectNodes(5, 2, 1);
            graph.connectNodes(5, 3, 1);

            adjacenyList(graph);
            adjacencyMatrix(graph);
            traversal(graph);
        }


        private static void traversal(Graph<int> graph)
        {
            Console.WriteLine("BREADTH FIRST TRAVERSAL \n ------");
            graph.dispGraphBFT();
            Console.WriteLine("DEPTH FIRST TRAVERSAL \n ------");
            graph.dispGraphDFT();
        }

        private static void adjacenyList(Graph<int> graph)
        {
            Console.WriteLine("ADJACENCY LIST \n ------");
            Dictionary<int, List<int>> dictionary = graph.getAdjacencyList();

            foreach (KeyValuePair<int, List<int>> pair in dictionary)
            {
                Console.Write("{0}: \t", pair.Key);
                foreach (int i in pair.Value)
                {
                    Console.Write("{0}\t", i);
                }
                Console.Write("\n");
            }
        }

        private static void adjacencyMatrix(Graph<int> graph)
        {
            Console.WriteLine("ADJACENCY MATRIX \n ------");
            int[,] matrix = graph.getAdjacencyMatrix();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i, j]} \t");
                }
                Console.WriteLine(";");
            }
        }
    }
}
