using System;
using System.Collections.Generic;

namespace Library.Tree
{
    public abstract class GameBoard : ICloneable
    {
        public enum GameState
        {
            PLAYER_TURN,
            AI_TURN,
            PLAYER_WIN,
            AI_WIN,
            DRAW,
            UNDEF
        }


        public struct Coordinate
        {
            public int row, col;
        }


        protected int[,] board;

        public abstract GameState CurrGameState { get; set; }
        public abstract string BoardString { get; }
        public abstract int[,] Board { get; set; }

        public GameBoard(int x, int y)
        {
            board = new int[x,y];
        }

        public GameBoard(int[,] boardIN)
        {
            board = boardIN;
        }

        /// <summary>
        /// Prints <see cref="Board"/> to Console
        /// </summary>
        public abstract void dispBoard();

        public abstract object Clone();

        /// <summary>
        /// Returns List of possible moves.
        /// Depends on <see cref="getEmptyCells()"/>
        /// </summary>
        /// <param name="isPlayer">Is the method getting the possible moves for the player or AI?</param>
        /// <returns></returns>
        public abstract List<Move> getPossibleMoves(bool isPlayer);

        public abstract List<Coordinate> getEmptyCells();

        /// <summary>
        /// Handles game progress. Should run continuously until end of game
        /// </summary>
        public abstract void Update();
    }
}
