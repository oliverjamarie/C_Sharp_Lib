using System;
using System.Collections.Generic;

namespace Library.Tree.Test_Implementations.TicTacToe
{
    public class TicTacToe : GameBoard
    {

        private new int[,] board = new int[3,3];
        public int playerUnit = 1;
        public int aiUnit = -1;

        GameState state;

        #region Properties
        public override int[,] Board
        {
            get => board;
            set
            {
                board = value;
            }
        }

        public override string BoardString
        {
            get
            {
                string boardStr = "";

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        boardStr += ((char)board[i, j]);
                    }
                    boardStr += '/';
                }

                return boardStr;
            }
        }

        /// <summary>
        /// 2D array representing the rows in the board
        /// </summary>
        public int [,] Rows
        {
            get
            {
                return board;
            }
        }


        /// <summary>
        /// 2D array representing the columns in the board
        /// </summary>
        public int[,] Columns
        {
            get
            {

                int[,] columns = new int[3,3];

                for (int col = 0; col < 3; col++)
                {
                    for (int row = 0; row < 3; row++)
                    {
                        columns[col,row] = board[row, col];
                    }
                    
                }

                return columns;
            }
        }

        /// <summary>
        /// 2D array representing the diagonals of the board
        /// </summary>
        public  int[,] Diagonals
        {
            get
            {

                int[,] diagonals =
                {
                    {board[0,0], board[1,1], board[2,2]},
                    {board[2,0], board[1,1], board[0,2]}
                };

                return diagonals;
            }
        }

        public override GameState CurrGameState
        {
            get
            {
                GameState gameState = endgameState(Columns);

                if (gameState != GameState.UNDEF)
                {
                    return gameState;
                }

                gameState = endgameState(Rows);
                if (gameState != GameState.UNDEF)
                {
                    return gameState;
                }

                gameState = endgameState(Diagonals);
                if (gameState != GameState.UNDEF)
                {
                    return gameState;
                }

                if (BoardString.Contains('0') == false)
                    return GameState.DRAW;

                return state;
            }
            set { state = value; }
        }
        #endregion

        #region Constructors
        public TicTacToe():base(3,3)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    board[y, x] = 0;
                }
            }

            state = GameState.PLAYER_TURN;
        }

        public TicTacToe(int[,] arr) : base(arr)
        {
            state = GameState.PLAYER_TURN;
        }

        #endregion

        public override void dispBoard()
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

        public override void Update()
        {
         
        }

        bool validMove(int move)
        {
            if (move > 9 || move < 1)
                return false;

            Coordinate moveCoord = convertMoveToCoordinate(move);
            

            Console.WriteLine($" Move Input : {move} \t Move's Column : {moveCoord.col} \t Move's Row : {moveCoord.row}");
            if(board[moveCoord.row, moveCoord.col] == 0)
            {
                if (CurrGameState == GameState.PLAYER_TURN)
                {
                    board[moveCoord.row, moveCoord.col] = 1;
                }
                else
                {
                    board[moveCoord.row, moveCoord.col] = -1;
                }
                return true;
            }
                
            return false;
        }

        // Can be done in O(1) but will implement later
        Coordinate convertMoveToCoordinate(int move)
        {
            int count = 1;
            Coordinate coord = new Coordinate();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if ( count == move)
                    {
                        coord.row = i;
                        coord.col = j;

                        return coord;
                    }
                    count++;
                }
            }

            coord.row = -1;
            coord.col = -1;

            return coord;
        }
        /// <summary>
        /// Cheks to see if the board is in an endgame state (win or a draw)
        /// </summary>
        /// <param name="arr">board</param>
        /// <returns></returns>
        private GameState endgameState(int[,] arr)
        {
            int sum = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    sum += arr[i, j];
                }

                if (sum == 3)
                    return GameState.PLAYER_WIN;
                if (sum == -3)
                    return GameState.AI_WIN;

                sum = 0;
            }

            return GameState.UNDEF;
        }

        public override List<Move> getPossibleMoves(bool isPlayer)
        {
            List<Move> moves = new List<Move>();
            List<Coordinate> coordinates = getEmptyCells();

            TicTacToe game = new TicTacToe();
            int val;

            if (isPlayer)
            {
                val = 1;
            }
            else
            {
                val = -1;
            }

            foreach (Coordinate coord in coordinates)
            {
                int[,] arr = (int[,])board.Clone();

                arr[coord.row, coord.col] = val;

                game = new TicTacToe(arr);

                Move move = new Move(game);

                moves.Add(move);
            }

            return moves;
        }

        /// <summary>
        /// Finds empty cells.
        /// </summary>
        /// <returns>List of Coordinates of empty cells</returns>
        public override List<Coordinate> getEmptyCells()
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row,col] == 0)
                    {
                        Coordinate coordinate = new Coordinate();
                        coordinate.row = row;
                        coordinate.col = col;

                        coordinates.Add(coordinate);
                    }
                }
            }

            return coordinates;
        }

        public override object Clone()
        {
            TicTacToe ticTacToe = new TicTacToe(board);
            ticTacToe.CurrGameState = CurrGameState;

            return ticTacToe;
        }
    }
}
