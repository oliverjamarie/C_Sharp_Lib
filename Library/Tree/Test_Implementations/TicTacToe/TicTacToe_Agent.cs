using System;
namespace C_Sharp_Lib.Library.Tree.Test_Implementations.TicTacToe
{
    public abstract class TicTacToe_Agent
    {
        protected TicTacToe game;

        public TicTacToe_Agent(TicTacToe ticTacToe)
        {
            game = ticTacToe;
        }

        public abstract int play();
    }
}
