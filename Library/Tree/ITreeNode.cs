using System;
using System.Collections.Generic;

namespace Library.Tree
{
    public interface ITreeNode<Move>
    {
        void addChild(Move data);
        void addChildren(List<ITreeNode<Move>> chidlrenIN);
        List<ITreeNode<Move>> getParents();
        int getID();

        ITreeNode<Move> Parent
        {
            get;
            set;
        }

        Move Data
        {
            get;
            set;
        }

        int ID
        {
            get;
        }

        List<ITreeNode<Move>> Children { get; }

    }
}