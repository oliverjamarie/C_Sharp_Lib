using System;
namespace C_Sharp_Lib.Library.Tree.Test_Implementations.TicTacToe
{
    public class TicTacToe_Agent_Player : TicTacToe_Agent
    {
        public TicTacToe_Agent_Player(TicTacToe ticTacToe):base(ticTacToe)
        {
        }

        public override int play()
        {
            int move = getPlayerInput();
            return move;
        }

        private int getPlayerInput()
        {
            Console.WriteLine("Player move ");
            char input = Console.ReadKey().KeyChar;
            int move = input - '0'; // converts ASCII input to single decimal number


            return move;
        }
    }
}
