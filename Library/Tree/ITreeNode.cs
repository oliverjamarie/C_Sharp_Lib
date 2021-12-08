using System;
using System.Collections.Generic;

namespace C_Sharp_Lib.Library.Tree
{
    public interface ITreeNode<T> where T : IComparable
    {
        //bool addChild(ITreeNode<T> child);
        bool addChild(T data);
        void addChildren(List<ITreeNode<T>> chidlrenIN);
        //bool Equals(object obj);
        //bool Equals(INode<T>other);
        List<ITreeNode<T>> getChildren();
        T getData();
        ITreeNode<T>getParent();
        List<ITreeNode<T>> getParents();
        int getID();
        int getScore();

        ITreeNode<T> Parent
        {
            get;
            set;
        }

        MiniMax NodeType
        {
            get;
        }

    }
}