using System;

namespace Library.Tree
{
    public class Move
    {
        GameBoard gameBoard;

        public Move(GameBoard gameBoard)
        {
            this.gameBoard = gameBoard;
        }

        /// <summary>
        /// Calculates score based off current <see cref="gameBoard"/>
        /// </summary>
        /// <returns>Calculated Score</returns>
        public int calculateScore()
        {
            return calculateScore(gameBoard);
        }

        /// <summary>
        /// Calculates score based off <paramref name="gameBoardIN"/>
        /// If the move does not end a game, the calculated
        /// score is the sum of cells <paramref name="gameBoardIN"/>
        /// </summary>
        /// <param name="gameBoardIN"></param>
        /// <returns>Calculated Score</returns>
        public int calculateScore(GameBoard gameBoardIN)
        {
            if (gameBoardIN.CurrGameState == GameBoard.GameState.DRAW)
                return 0;

            if (gameBoardIN.CurrGameState == GameBoard.GameState.AI_WIN)
                return 10;

            if (gameBoardIN.CurrGameState == GameBoard.GameState.PLAYER_WIN)
                return -10;

            // If the game is still in play, the score is determined by the remaining peices in the board
            int sum = 0;

            foreach(int num in gameBoardIN.Board)
            {
                sum += num;
            }
            
            return sum;
        }

        
    }
}
