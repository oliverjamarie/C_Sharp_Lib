using System;
using Library.Graph;
using Library.Algorithms;
using System.Collections;
using System.Collections.Generic;

namespace Library
{
    public static class Driver
    {
        public static void Main()
        {
            testPreferredAttach();
            Console.WriteLine("Done! Write a character and press ENTER to finish");
            Console.ReadLine();
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

        private static void testGraph3()
        {
            Graph<int> graph = new Graph<int>();

            for (int i = 1; i <= 8; i++)
            {
                graph.insert(i);
            }

            graph.connectNodes(1, 2, 9);
            graph.connectNodes(1, 3, 5);

            graph.connectNodes(2, 1, 3);
            graph.connectNodes(2, 4, 18);

            graph.connectNodes(3, 4, 12);

            graph.connectNodes(4, 8, 8);

            graph.connectNodes(5, 4, 9);
            graph.connectNodes(5, 6, 2);
            graph.connectNodes(5, 7, 5);
            graph.connectNodes(5, 8, 3);

            graph.connectNodes(6, 7, 1);

            graph.connectNodes(7, 5, 4);
            graph.connectNodes(7, 8, 6);

            graph.connectNodes(8, 5, 3);

            adjacenyList(graph);
            adjacencyMatrix(graph);
            traversal(graph);
        }

        private static void testGraphConstructor()
        {

            Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
            List<int> list = new List<int>();

            for (int i = 1; i <= 5; i++)
            {
                list.Add(i);
            }

            dict.Add(0, list);

            list = new List<int>();
            list.Add(0);

            for (int i = 1; i <= 5; i++)
            {
                dict.Add(i, list);
            }

            foreach(KeyValuePair<int, List<int>> keyValuePair in dict)
            {
                Console.Write(keyValuePair.Key);

                foreach(int x in keyValuePair.Value)
                {
                    Console.Write($"-- {x} \t");
                }
                Console.WriteLine();
            }

            Graph<int> graph = new Graph<int>(dict);
            
            traversal(graph);
            adjacencyMatrix(graph);
        }

        private static void testGraphConstructor2()
        {
            Dictionary<int, Dictionary<int, double>> dict = new Dictionary<int, Dictionary<int, double>>();
            Dictionary<int, double> adjList = new Dictionary<int, double>();


            // 0
            adjList.Add(1, .8);
            adjList.Add(2, .5);
            adjList.Add(3, .1);
            adjList.Add(4, .2);
            adjList.Add(5, .7);

            dict.Add(0, adjList);

            // 1
            adjList = new Dictionary<int, double>();
            adjList.Add(0, .81);
            dict.Add(1, adjList);

            // 2
            adjList = new Dictionary<int, double>();
            adjList.Add(0, .51);
            dict.Add(2, adjList);

            // 3
            adjList = new Dictionary<int, double>();
            adjList.Add(0, .11);
            dict.Add(3, adjList);

            // 4
            adjList = new Dictionary<int, double>();
            adjList.Add(0, .21);
            dict.Add(4, adjList);

            // 5
            adjList = new Dictionary<int, double>();
            adjList.Add(0, .71);
            dict.Add(5, adjList);

            Graph<int> graph = new Graph<int>(dict);

            adjacencyMatrix(graph);
            adjacenyList(graph);
            traversal(graph);
        }


        private static void traversal(Graph<int> graph)
        {
            Console.WriteLine("BREADTH FIRST TRAVERSAL \n ------");
            graph.BFT();
            Console.WriteLine("DEPTH FIRST TRAVERSAL \n ------");
            graph.DFT();
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

        private static void weightedAdjacenyList(Graph<int> graph)
        {
            Console.WriteLine("WEIGHTED ADJACENCY LIST \n ------");
            Dictionary<int, Dictionary<int, double>> dictionary = graph.getWeightedAdjacencyList();

            foreach (KeyValuePair<int, Dictionary<int,double>> pair in dictionary)
            {
                Console.Write("{0}: \t", pair.Key);
                foreach(KeyValuePair<int, double> subPair in pair.Value)
                {
                    Console.Write($"({subPair.Key} , {subPair.Value}) \t");
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

        private static void testPreferredAttach()
        {
            Dictionary<int, Dictionary<int, double>> dict = new Dictionary<int, Dictionary<int, double>>();
            Dictionary<int, double> adjList = new Dictionary<int, double>();

            // E
            adjList.Add(0, 0.2);
            adjList.Add(1, 0.3);
            adjList.Add(2, 0.3);
            dict.Add(0, adjList);

            // Con
            adjList = new Dictionary<int, double>();
            adjList.Add(0, 0.3);
            adjList.Add(1, 0.3);
            adjList.Add(2, 0.4);
            dict.Add(1, adjList);

            // Cov
            adjList = new Dictionary<int, double>();
            adjList.Add(0, 0.2);
            adjList.Add(1, 0.5);
            adjList.Add(2, 0.2);
            dict.Add(2, adjList);

            PreferredAttachment<int> preferredAttachment = new PreferredAttachment<int>(dict);
            preferredAttachment.setCostModifier(0.1);

            weightedAdjacenyList(preferredAttachment.graph);

            for(int i = 0; i < 5; i++)
            {
                Queue<int> queue = preferredAttachment.genQueue(30);
                Console.WriteLine($"\n\nRAND LIST {i}");
                foreach (int num in queue)
                {
                    if (num == 0)
                    {
                        Console.Write("E \t");
                    }
                    else if (num == 1)
                    {
                        Console.Write("Con \t");
                    }
                    else
                    {
                        Console.Write("Cov \t");
                    }
                }

                Console.WriteLine();
            }

        }
    }
}

