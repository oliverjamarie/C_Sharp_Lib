using System;
using System.Collections.Generic;


namespace C_Sharp_Lib.Library.Tree
{
    public partial class GameTree<T>
    {
        protected class Node : ITreeNode<T>
        {

            protected List<ITreeNode<T>> children;
            protected T data;
            protected ITreeNode<T> parent;
            private static int countNodes = 0;
            protected int id;
            private MiniMax nodeType;

            public static int CountNodes
            {
                get
                {
                    return countNodes;
                }
            }

            public MiniMax NodeType
            {
                get { return nodeType; }
            }

            public ITreeNode<T> Parent
            {
                get
                {
                    return parent;
                }
                set
                {
                    if (Parent.NodeType == MiniMax.MAX_NODE)
                        nodeType = MiniMax.MIN_NODE;
                    else
                        nodeType = MiniMax.MAX_NODE;

                    parent = Parent;
                }
            }

            public Node()
            {
                children = new List<ITreeNode<T>>();
                data = default(T);
                parent = null;
                nodeType = MiniMax.MAX_NODE;
                id = countNodes++;
            }

            public Node(T data) : this()
            {
                this.data = data;
            }

            public Node(T data, List<Node> children) : this(data)
            {
                this.children.AddRange(children);
            }

            public Node(T data, Node parent) : this(data)
            {
                this.parent = parent;
                if (parent.nodeType == MiniMax.MAX_NODE)
                    nodeType = MiniMax.MIN_NODE;
                else
                    nodeType = MiniMax.MAX_NODE;
            }

            public List<ITreeNode<T>> getChildren()
            {
                return children;
            }

            public T getData()
            {
                return data;
            }

            public ITreeNode<T> getParent()
            {
                return parent;
            }

            public bool addChild(ITreeNode<T> child)
            {
                if (getParents().Contains(child))
                    return false;

                children.Add(child);
                child.Parent = this;

                return true;
            }

            public bool addChild(T data)
            {
                Node node = new Node(data);
                return addChild(node);
            }

            public void addChildren(List<ITreeNode<T>> chidlrenIN)
            {
                foreach (Node node in chidlrenIN)
                {
                    addChild(node);
                }
            }

            public List<ITreeNode<T>> getParents()
            {
                List<ITreeNode<T>> parents = new List<ITreeNode<T>>();
                ITreeNode<T> curr = this;

                while (curr.getParent() != null)
                {
                    parents.Add(curr.getParent());
                    curr = curr.getParent();
                }

                return parents;
            }

            public override bool Equals(object obj)
            {
                if (obj.GetType() == typeof(Node))
                    return this.Equals(obj);

                return false;
            }

            public bool Equals(ITreeNode<T> other)
            {
                return this.id == other.getID();
            }

            public int getID()
            {
                return this.id;
            }

            public int getScore()
            {
                return data.getScore();
            }
        }
    }
}
