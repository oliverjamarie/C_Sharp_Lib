using System;
using System.Collections.Generic;

namespace C_Sharp_Lib.Library.Tree
{
    public partial class Tree<T> where T : IComparable
    {
        private Node root;

        public Tree()
        {
            root = new Node();
        }

        public Tree (T data) 
        {
            root = new Node(data);
        }

        public List<INode<T>> DFT()
        {
            List<INode<T>> tree = new List<INode<T>>();

            if (root == null)
                return tree;

            DFT(root, tree);

            return tree;
        }

        private void DFT(Node source, List<INode<T>> list)
        {
            list.Add(source);

            foreach(Node child in source.getChildren())
            {
                DFT(child, list);
            }
        }

        public List<INode<T>> BFT()
        {
            List<INode<T>> tree = new List<INode<T>>();

            if (root == null)
                return tree;

            return tree;
        }

        private void BFT(Node source, List<INode<T>> list)
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
            Node parent = getNode(parentID, root);

            return parent.addChild(data);            
        }

        public INode<T> getNode(int nodeID)
        {
            return getNode(nodeID, root);
        }

        private Node getNode(int nodeID, Node node)
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
    }
}
