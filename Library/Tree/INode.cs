using System;
using System.Collections.Generic;

namespace C_Sharp_Lib.Library.Tree
{
    public interface INode<T> where T : IComparable
    {
        bool addChild(INode<T>child);
        bool addChild(T data);
        void addChildren(List<INode<T>> chidlrenIN);
        bool Equals(object obj);
        bool Equals(INode<T>other);
        List<INode<T>> getChildren();
        T getData();
        INode<T>getParent();
        List<INode<T>> getParents();
        void setParent(INode<T> parent);
        int getID();
    }
}