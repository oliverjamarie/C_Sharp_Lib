using System;
using System.Collections.Generic;


namespace Library.Tree
{
    public partial class GameTree<Move>
    {
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        public class Node : IMiniMax<Move>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        {

            protected List<ITreeNode<Move>> children;
            protected Move data;
            protected IMiniMax<Move> parent;
            private static int countNodes = 0;
            private static IMiniMax<Move>.MiniMaxAlgoMode algoMode;
            protected int id;
            private IMiniMax<Move>.MiniMax nodeType;

            #region Properties 
            public static int CountNodes
            {
                get
                {
                    return countNodes;
                }
            }

            public List<ITreeNode<Move>> Children
            {
                get => children;
            }

            public Move Data
            {
                get
                {
                    return data;
                }
                set
                {
                    data = value;
                }
            }

            public int ID
            {
                get
                {
                    return id;
                }
            }

            public IMiniMax<Move>.MiniMax NodeType
            {
                get { return nodeType; }
            }

            public IMiniMax<Move> Parent
            {
                get
                {
                    return parent;
                }
                set
                {
                    if (Parent.NodeType == IMiniMax<Move>.MiniMax.MAX_NODE)
                        nodeType = IMiniMax<Move>.MiniMax.MIN_NODE;
                    else
                        nodeType = IMiniMax<Move>.MiniMax.MAX_NODE;

                    parent = value;
                }
            }

            public IMiniMax<Move>.MiniMaxAlgoMode AlgoMode
            {
                get
                {
                    return algoMode;
                }
                set
                {
                    algoMode = value;
                }
            }
            protected bool IsTerminal
            {
                get
                {
                    return children.Count == 0;
                }
            }

            ITreeNode<Move> ITreeNode<Move>.Parent {
                get => ((ITreeNode<Move>)Parent).Parent;
                set => ((ITreeNode<Move>)Parent).Parent = value;
            }
            #endregion

            #region Constructors
            public Node()
            {
                children = new List<ITreeNode<Move>>();
                data = default;
                parent = null;
                nodeType = IMiniMax<Move>.MiniMax.MAX_NODE;
                id = countNodes++;
            }

            public Node(Move data) : this()
            {
                this.data = data;
            }

            public Node(Move data, List<Node> children) : this(data)
            {
                this.children.AddRange(children);
            }

            public Node(Move data, Node parent) : this(data)
            {
                this.parent = parent;
                if (parent.nodeType == IMiniMax<Move>.MiniMax.MAX_NODE)
                    nodeType = IMiniMax<Move>.MiniMax.MIN_NODE;
                else
                    nodeType = IMiniMax<Move>.MiniMax.MAX_NODE;
            }

            #endregion


            public void addChild(ITreeNode<Move> child)
            {
                children.Add(child);
                child.Parent = this;
            }

            public void addChild(Move data)
            {
                Node node = new Node(data);
                addChild(node);
            }

            public void addChildren(List<ITreeNode<Move>> chidlrenIN)
            {
                foreach (Node node in chidlrenIN)
                {
                    addChild(node);
                }
            }

            public List<ITreeNode<Move>> getParents()
            {
                List<ITreeNode<Move>> parents = new List<ITreeNode<Move>>();
                ITreeNode<Move> curr = this;

                while (curr.Parent != null)
                {
                    parents.Add(curr.Parent);
                    curr = curr.Parent;
                }

                return parents;
            }

            public override bool Equals(object obj)
            {
                if (obj.GetType() == typeof(Node))
                    return this.Equals(obj);

                return false;
            }

            public bool Equals(ITreeNode<Move> other)
            {
                return this.id == other.getID();
            }

            public int getID()
            {
                return this.id;
            }

            public int generateScore()
            {
                return Parent.generateScore();
            }

        }
    }
}
