using System;
using System.Collections.Generic;

namespace Library.Tree
{
    /// <summary>
    /// Class used for games where you want to implement MiniMax
    /// </summary>
    /// <typeparam name="Move">
    /// Creating a child of <typeparamref name="Move"/>
    /// allows you to create a MiniMax AI that can work multiple games
    /// </typeparam>
    public partial class GameTree<Move> 
    {
        private Node root;

        public GameTree()
        {
            root = null;
        }

        public GameTree (Move data) 
        {
            root = new Node(data);
        }

        /// <summary>
        /// Depth first traversal. Dependant on <see cref="DFT(Node, List{ITreeNode{Move}})"/>
        /// </summary>
        /// <returns>List of all elements in the tree</returns>
        public List<ITreeNode<Move>> DFT()
        {
            List<ITreeNode<Move>> tree = new List<ITreeNode<Move>>();

            if (root == null)
                return tree;

            DFT(root, tree);

            return tree;
        }

        /// <summary>
        /// Helper method for <see cref="DFT"/>
        /// </summary>
        /// <param name="source">Current node being visited</param>
        /// <param name="list">List of nodes visited so far</param>
        private void DFT(Node source, List<ITreeNode<Move>> list)
        {
            list.Add((ITreeNode<Move>)source);

            foreach(Node child in source.Children)
            {
                DFT(child, list);
            }
        }

        /// <summary>
        /// Breadth first traversal.  Dependant on <see cref="BFT(Node, List{ITreeNode{Move}})"/>
        /// </summary>
        /// <returns></returns>
        public List<ITreeNode<Move>> BFT()
        {
            List<ITreeNode<Move>> tree = new List<ITreeNode<Move>>();

            if (root == null)
                return tree;

            return tree;
        }

        /// <summary>
        /// Helper method for <see cref="BFT"/>
        /// </summary>
        /// <param name="source">Current node being visited</param>
        /// <param name="list">List of nodes visited so far</param>
        private void BFT(Node source, List<ITreeNode<Move>> list)
        {
            if (source == null)
                return;

            foreach(Node child in source.Children){
                BFT(child,list);
                list.Add((ITreeNode<Move>)source);
            }
        }

        /// <summary>
        /// Insert a <see cref="Move"/> into the tree
        /// </summary>
        /// <param name="data">Move to insert in tree</param>
        /// <param name="parentID">ID of the parent node we want to connect this to</param>
        public void insert(Move data, int parentID)
        {
            if (root == null)
            {
                root = new Node(data);
                return;
            }

            Node parent = getNode(parentID, root);

            if (parent == null)
                return ;

            parent.addChild(data);            
        }

        /// <summary>
        /// Finds node in tree based on its ID
        /// </summary>
        /// <param name="nodeID">ID of the node we want</param>
        /// <returns>Found node</returns>
        public ITreeNode<Move> getNode(int nodeID)
        {
            return getNode(nodeID, root);
        }

        /// <summary>
        /// Searches through tree looking for a node based on its ID
        /// </summary>
        /// <param name="nodeID">ID of Node we want</param>
        /// <param name="node">Current node we're visiting</param>
        /// <returns>Found node</returns>
        private Node getNode(int nodeID, Node node)
        {
            if (node == null)
                return null;

            if (node.getID() == nodeID)
                return node;

            foreach(Node child in node.Children)
            {
                if (getNode(nodeID, child) != null)
                    return node;
            }

            return null;
        }

        
    }
}
