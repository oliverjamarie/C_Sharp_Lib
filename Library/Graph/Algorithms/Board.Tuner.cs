using System;

namespace Library.Graph.Algorithms
{

    public partial class Board
    {
        protected class Tuner
        {
            double targetMapAccessability;
            double maxMapAccessability;
            double minMapAccessability;
            double targetAvgCost;
            double maxAvgCost;
            double minAvgCost;
            bool tuned;
            Board board;

            public Tuner(Board boardIN)
            {
                board = boardIN;
            }

            
            public double MapAccessability
            {
                get => targetMapAccessability;

                set
                {
                    if (value < 0 || value > 1.0)
                        throw new InvalidTunerParameterException();

                    targetMapAccessability = value;
                    maxMapAccessability = value + (value * 0.01);
                    minMapAccessability = value - (value * 0.01);

                    Console.WriteLine($"Max {maxMapAccessability}  Min {minMapAccessability}");
                }
            }

            /// <summary>
            /// Average cost to travel from top row to bottom row
            /// </summary>
            public double AvgCost
            {
                get => targetAvgCost;
                set
                {
                    targetAvgCost = value;
                    maxAvgCost = value + (value * 0.01);
                    minAvgCost = value - (value * 0.01);
                }
            }


            public void Tune()
            {
                tuned = false;
                int count = 0;


                while (!tuned)
                {
                    tuned = true;
                    board.generateBoard();

                    if (board.WalkableSurface > maxMapAccessability)
                    {
                        tuned = false;
                        tuneMapAccess(false);
                    }

                    else if (board.WalkableSurface < minMapAccessability)
                    {
                        tuned = false;
                        tuneMapAccess(true);
                    }

                    if (board.AverageCost > maxAvgCost)
                    {
                        tuned = false;
                        tuneAvgCost(false);
                    }
                    else if (board.AverageCost < minAvgCost)
                    {
                        tuned = false;
                        tuneAvgCost(true);
                    }


                    if (count < 1000)
                    {
                        count++;
                    }
                    else
                        break;

                    board.generateBoard();
                }
            }


            /// <summary>
            /// Helper method used to increase or decrease <see cref="MapAccessability"/>
            /// </summary>
            /// <param name="increase">Increase <see cref="MapAccessability"/>?</param>
            private void tuneMapAccess(bool increase)
            {
                board.preferredAttachment.tuneConnection(board.empty, board.empty, increase);
                board.preferredAttachment.tuneConnection(board.empty, board.conceal, increase);
                board.preferredAttachment.tuneConnection(board.empty, board.cover, !increase);

                board.preferredAttachment.tuneConnection(board.conceal, board.conceal, increase);
                board.preferredAttachment.tuneConnection(board.conceal, board.empty, increase);
                board.preferredAttachment.tuneConnection(board.conceal, board.cover, !increase);

                board.preferredAttachment.tuneConnection(board.cover, board.cover, !increase);
                board.preferredAttachment.tuneConnection(board.cover, board.conceal, increase);
                board.preferredAttachment.tuneConnection(board.cover, board.empty, increase);
            }


            private void tuneAvgCost(bool increase)
            {
                board.preferredAttachment.tuneConnection(board.empty, board.empty, increase);
                board.preferredAttachment.tuneConnection(board.empty, board.conceal, !increase);

                board.preferredAttachment.tuneConnection(board.conceal, board.empty, increase);
                board.preferredAttachment.tuneConnection(board.conceal, board.conceal, !increase);
            }
        }
    }

}
