using System;

namespace Library.Tree
{
    public interface IMiniMax<Move> : ITreeNode<Move>
    {
        enum MiniMaxAlgoMode
        {
            DEFAULT,
            ALPHA_BETA_PRUNE
        }

        enum MiniMax
        {
            MAX_NODE,
            MIN_NODE,
            UNDEF
        }

        MiniMax NodeType
        {
            get;
        }

        MiniMaxAlgoMode AlgoMode
        {
            get;
            set;
        }
        
        // "new" used overidde parent interface
        new IMiniMax<Move> Parent
        {
            get;
            set;
        }

        int generateScore();
    }
}
