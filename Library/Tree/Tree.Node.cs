using System;
using System.Collections.Generic;


namespace C_Sharp_Lib.Library.Tree
{
    public partial class Tree<T>
    {
        protected class Node : INode<T>
        {
            List<INode<T>> children;
            T data;
            INode<T> parent;
            protected static int countNodes = 0;
            int id;

            public Node()
            {
                children = new List<INode<T>>();
                data = default(T);
                parent = null;
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
            }

            public List<INode<T>> getChildren()
            {
                return children;
            }

            public T getData()
            {
                return data;
            }

            public INode<T> getParent()
            {
                return parent;
            }

            public void setParent(INode<T> parent)
            {
                this.parent = parent;
            }

            public bool addChild(INode<T> child)
            {
                if (getParents().Contains(child))
                    return false;

                children.Add(child);
                child.setParent(this);

                return true;
            }

            public bool addChild(T data)
            {
                Node node = new Node(data);
                return addChild(node);
            }

            public void addChildren(List<INode<T>> chidlrenIN)
            {
                foreach (Node node in chidlrenIN)
                {
                    addChild(node);
                }
            }

            public List<INode<T>> getParents()
            {
                List<INode<T>> parents = new List<INode<T>>();
                INode<T> curr = this;

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

            public bool Equals(INode<T> other)
            {
                return this.id == other.getID();
            }

            public int getID()
            {
                return this.id;
            }
        }
    }
}
