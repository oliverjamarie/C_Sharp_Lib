using System;
using System.Collections.Generic;

namespace C_Sharp_Lib.Library.Tree
{
    public partial class GameTree<T> where T : IComparable, IScore
    {
        private ITreeNode<T> root;

        public GameTree()
        {
            root = null;
        }

        public GameTree (T data) 
        {
            root = new Node(data);
        }

        public List<ITreeNode<T>> DFT()
        {
            List<ITreeNode<T>> tree = new List<ITreeNode<T>>();

            if (root == null)
                return tree;

            DFT(root, tree);

            return tree;
        }

        private void DFT(ITreeNode<T> source, List<ITreeNode<T>> list)
        {
            list.Add(source);

            foreach(ITreeNode<T> child in source.getChildren())
            {
                DFT(child, list);
            }
        }

        public List<ITreeNode<T>> BFT()
        {
            List<ITreeNode<T>> tree = new List<ITreeNode<T>>();

            if (root == null)
                return tree;

            return tree;
        }

        private void BFT(Node source, List<ITreeNode<T>> list)
        {
            if (source == null)
                return;

            foreach(Node child in source.getChildren()){
                BFT(child,list);
                list.Add(source);
            }
        }

        public bool insert(T data, int parentID)
        {
            if (root == null)
            {
                root = new Node(data);
            }

            ITreeNode<T> parent = getNode(parentID, root);

            if (parent == null)
                return false;

            return parent.addChild(data);            
        }

        public ITreeNode<T> getNode(int nodeID)
        {
            return getNode(nodeID, root);
        }

        private ITreeNode<T> getNode(int nodeID, ITreeNode<T> node)
        {
            if (node == null)
                return null;

            if (node.getID() == nodeID)
                return node;

            foreach(Node child in node.getChildren())
            {
                if (getNode(nodeID, child) != null)
                    return node;
            }

            return null;
        }

        public List<ITreeNode<T>> minimax(int depth, MiniMaxAlgoMode miniMaxAlgoMode = MiniMaxAlgoMode.DEFAULT)
        {
            ITreeNode<T> terminalNode;

            if (miniMaxAlgoMode == MiniMaxAlgoMode.DEFAULT)
               terminalNode = minimax(root, depth);
            else
                terminalNode = minimax(root, depth, int.MinValue, int.MaxValue);

            List<ITreeNode<T>> nodes = new List<ITreeNode<T>>();
            nodes.Add(terminalNode);
            nodes.AddRange(terminalNode.getParents());

            return nodes;
        }

        private ITreeNode<T> minimax(ITreeNode<T> curr, int depthRemain, int alpha, int beta)
        {
            ITreeNode<T> terminalNode = curr;

            if (curr == null)
            {
                return null;
            }

            if (depthRemain <= 0 || curr.getChildren().Count == 0)
            {
                return curr;
            }


            foreach (Node child in curr.getChildren())
            {
                ITreeNode<T> node = minimax(child, depthRemain - 1, alpha, beta);
                if (curr.NodeType == MiniMax.MAX_NODE)
                {
                    if (beta > node.getScore())
                    {
                        terminalNode = node;
                        beta = node.getScore();
                        if (alpha >= beta)
                        {
                            Console.WriteLine("Beta was less than Alpha.....PRUNED BRANCH");
                            break;
                        }
                    }

                }
                else
                {
                    if (alpha < node.getScore())
                    {
                        terminalNode = node;
                        alpha = node.getScore();
                        if (alpha >= beta)
                        {
                            Console.WriteLine("Beta was less than Alpha.....PRUNED BRANCH");
                            break;
                        }
                    }
                }
            }

            return terminalNode;

        }

        private ITreeNode<T> minimax(ITreeNode<T> curr, int depthRemain)
        {
            ITreeNode<T> terminalNode = curr;

            if (curr == null)
            {
                return null ;
            }

            if (depthRemain <= 0 || curr.getChildren().Count == 0)
            {
                return curr;
            }


            foreach (Node child in curr.getChildren())
            {
                ITreeNode<T> node = minimax(child, depthRemain - 1);
                
                int score;

                if (curr.NodeType == MiniMax.MAX_NODE)
                {
                    score = int.MinValue;
                    if (node.getScore() >= score)
                    {
                        terminalNode = node;
                    }
                }
                else
                {
                    score = int.MaxValue;
                    if (node.getScore() <= score)
                    {
                        terminalNode = node;
                    }
                }
            }
            return terminalNode;
        }

        private ITreeNode<T> Max(ITreeNode<T> node1, ITreeNode<T> node2)
        {
            if (node1.getScore() > node2.getScore())
                return node1;
            else
                return node2;
        }

        private ITreeNode<T> Min(ITreeNode<T> node1, ITreeNode<T> node2)
        {
            if (node1.getScore() < node2.getScore())
                return node1;
            else
                return node2;
        }
    }
}
