using System;
using System.Collections.Generic;

namespace C_Sharp_Lib.Library.Tree.Test_Implementations.TicTacToe
{
    public class TicTacToe
    {

        public enum GameState
        {
            PLAYER_TURN,
            AI_TURN,
            GAME_OVER,
            PLAYER_WIN,
            AI_WIN
        }

        private char[,] board = new char[3,3];
        public const char PlayerChar = 'O';
        public const char aiChar = 'X';
        public const char emptyChar = '_';
        TicTacToe_Agent_Player player;
        TicTacToe_Agent_AI ai;
        GameState state;
        bool isGameOver;

        public char[,] Board
        {
            get => board;
        }

        public string BoardString
        {
            get
            {
                string boardStr = "";

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        boardStr += board[i, j];
                    }
                }

                return boardStr;
            }
        }

        public string[] Rows
        {
            get
            {
                return BoardString.Split('/');
            }
        }

        public string[] Columns
        {
            get
            {
                string[] columns = new string[3];
                
                for (int col = 0; col < 3; col++)
                {
                    string column = "";

                    for (int row = 0; row < 3; row++)
                    {
                        column += board[row, col];
                    }
                    columns[col] = column;
                }

                return columns;
            }
        }

        public  string[] Diagonals
        {
            get
            {
                string[] diagonals = new string[2];
                diagonals[0] = "";
                diagonals[1] = "";
                for (int row = 2; row >= 0; row--)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (row == col)
                        {
                            diagonals[0] += board[row, col];
                        }

                        diagonals[1] += board[row, col];
                    }
                }

                return diagonals;
            }
        }

        public GameState State
        {
            get
            {
                List<string> list = new List<string>();
                list.AddRange(Columns);
                list.AddRange(Rows);
                list.AddRange(Diagonals);

                foreach (string set in list)
                {
                    if (set.Equals("OOO"))
                    {
                        isGameOver = true;
                        state = GameState.PLAYER_WIN;
                        return GameState.PLAYER_WIN;
                    }


                    if (set.Equals("XXX"))
                    {
                        isGameOver = true;
                        state = GameState.AI_WIN;
                        return GameState.AI_WIN;
                    }
                        
                }

                if (BoardString.Contains('_') == false)
                {
                    isGameOver = true;
                    state = GameState.GAME_OVER;
                    return GameState.GAME_OVER;
                }
                    

                return state;
            }
            set { state = State; }
        }

        

        public TicTacToe()
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    board[y, x] = emptyChar;
                }
            }

            player = new TicTacToe_Agent_Player(this);
            ai = new TicTacToe_Agent_AI(this);
            state = GameState.PLAYER_TURN;
            isGameOver = false;
            Update();
        }

        public void dispBoard()
        {
            int count = 1;

            for (int y = 0; y < 3; y++)
            {
                string output = "";
                for (int x = 0; x < 3; x++)
                {
                    output += board[y, x] + "\t";
                }

                for (int x = 0; x < 3; x++)
                {
                    output += count + "\t";
                    count++;
                }

                Console.WriteLine(output);
            }
        }

        void Update()
        {
            TicTacToe_Agent agent = player; // used to keep track of who's turn it is

            while (!isGameOver)
            {
                dispBoard();

                int move = agent.play();

                validMove(move);

                //if (State == GameState.PLAYER_TURN)
                //{
                //    agent = ai;
                //    State = GameState.AI_TURN;
                //}
                //else
                //{
                //    agent = player;
                //    State = GameState.PLAYER_TURN;
                //}

                
            }
        }

        bool validMove(int move)
        {
            if (move > 9 || move < 1)
                return false;

            (int row, int col) moveCoord = convertMoveToCoordinates(move);
            

            

            Console.WriteLine($" Move Input : {move} \t Move's Column : {moveCoord.col} \t Move's Row : {moveCoord.row}");
            if(board[moveCoord.row, moveCoord.col] == '_')
            {
                if (State == GameState.PLAYER_TURN)
                {
                    board[moveCoord.row, moveCoord.col] = 'O';
                }
                else
                {
                    board[moveCoord.row, moveCoord.col] = 'X';
                }
                return true;
            }
                
            return false;
        }

        // Can be done in O(1) but won't rn
        (int row, int col) convertMoveToCoordinates(int move)
        {
            int count = 1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if ( count == move)
                    {
                        return (i, j);
                    }
                    count++;
                }
            }

            return (-1,-1);
        }

    }
}
