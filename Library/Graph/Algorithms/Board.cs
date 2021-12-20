using System;
using System.Collections.Generic;
using Library.Graph.Algorithms.Pathfinding;

namespace Library.Graph.Algorithms
{
    
    public struct CellType : IComparable,  IEquatable<CellType>
    {
        public bool navigatable;
        public double cost;
        public string displayName;

        public CellType(bool navigatableIN, double costIN, string displayNameIN)
        {
            navigatable = navigatableIN;
            cost = costIN;
            displayName = displayNameIN;
        }

        public int CompareTo(object obj)
        {
            if (obj is CellType)
            {
                CellType comp = (CellType)obj;

                if (comp.navigatable == this.navigatable)
                {
                    return cost.CompareTo(comp.cost);
                }
            }

            return int.MinValue;
        }

        public override bool Equals(object obj)
        {
            return obj is CellType type && Equals(type);
        }

        public bool Equals(CellType other)
        {
            return navigatable == other.navigatable &&
                   cost == other.cost;
        }

        public override string ToString()
        {
            return displayName;
        }

    }

    public struct Cell : IComparable
    {
        public CellType cellType;
        private static int countCells = 0;
        private int id;

        public int ID { get => id; }

        public Cell(CellType cellTypeIN)
        {
            cellType = cellTypeIN;
            id = countCells++;
        }

        public int CompareTo(object obj)
        {
            if (obj is Cell)
            {
                Cell comp = (Cell)obj;

                return ID.CompareTo(comp.ID);
            }

            throw new Exception("Cannot Compare To Given Object");
        }

        
    }

    /// <summary>
    /// Used for board generation
    /// </summary>
    public partial class Board
    {
        CellType empty, conceal, cover;
        Cell[,] cells;
        
        protected PreferredAttachment<CellType> preferredAttachment;

        int size;

        #region Properties
        public int Size { get => size;}

        public double WalkableSurface
        {
            get
            {
                int count = countCellType(cover);

                return 1.0 - ((double)count / (size * size));
            }
        }

        public PreferredAttachment<CellType> PreferredAttachment
        {
            get => preferredAttachment;
            set => preferredAttachment = value;
        }

        /// <summary>
        /// Used to get the average cost to travel from every cell in the top row
        /// to every cell in the bottom row
        /// </summary>
        public double AverageCost
        {
            get
            {
                Graph<Cell> graph = convertBoardToGraph();
                double totalCost = 0;
                Dijkstra<Cell> dijkstra = new Dijkstra<Cell>(graph);

                

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (graph.contains(cells[0, i]) && graph.contains(cells[size - 1, j]))
                            totalCost += dijkstra.ShortestDistance(cells[0, i], cells[size - 1, j]);
                    }
                }

                return totalCost / size;
            }
        }

        #endregion Properties

        public Board(int sizeIN)
        {
            empty = new CellType(true, 1.0, "Emp");
            conceal = new CellType(true, 2.0, "Con");
            cover = new CellType(false, int.MaxValue, "Cov");

            size = sizeIN;

            cells = new Cell[Size, Size];

            #region graph Setup
            Graph<CellType> graph = new Graph<CellType>();

            double defaultValue = 0.33;

            graph.insert(empty);
            graph.insert(conceal);
            graph.insert(cover);

            graph.connectNodes(empty, empty, defaultValue);
            graph.connectNodes(empty, conceal, defaultValue);
            graph.connectNodes(empty, cover, defaultValue);

            graph.connectNodes(conceal, conceal, defaultValue);
            graph.connectNodes(conceal, empty, defaultValue);
            graph.connectNodes(conceal, cover, defaultValue);

            graph.connectNodes(cover, cover, defaultValue);
            graph.connectNodes(cover, conceal, defaultValue);
            graph.connectNodes(cover, empty, defaultValue);
            #endregion graph Setup

            PreferredAttachment = new PreferredAttachment<CellType>(graph);   
        }

        public Cell[,] generateBoard()
        {
            cells = new Cell[Size, Size];

            for(int i = 0; i < Size; i++)
            {
                for(int j = 0; j < Size; j++)
                {
                    CellType cell;

                    if (i == 0 || i == Size - 1)
                        cell = empty;
                    else
                        cell = PreferredAttachment.getNext();

                    cells[i, j] = new Cell(cell);
                }
            }

            return cells;
        }


        /// <summary>
        /// Tunes settings to the generated the desired board
        /// </summary>
        /// <param name="mapAccessAbiliity">Portion of tiles that can navigate from the top row to the bottom row</param>
        /// <param name="avgCost">Average cost for a cell to travel from top row to bottom row</param>
        public void tunePrefAttach(double mapAccessAbility, double avgCost)
        {
            Tuner tuner = new Tuner(this);
            tuner.MapAccessability = mapAccessAbility;
            tuner.AvgCost = avgCost;

            tuner.Tune();
        }


        /// <summary>
        /// Counts the number of given <see cref="CellType"/> in the board
        /// </summary>
        /// <param name="cellType"><see cref="CellType"/> we want to count</param>
        /// <returns></returns>
        public int countCellType(CellType cellType)
        {
            int count = 0;

            foreach(Cell cell in cells)
            {

                if (cell.cellType.Equals(cellType))
                {
                    count++;
                }
            }

            return count;
        }


        /// <summary>
        /// Writes board to Console
        /// </summary>
        public void dispBoard()
        {
            Console.WriteLine($"Board of size {size}");
            for(int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write("{0}\t", cells[i, j].cellType);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Helper method for <see cref="AverageCost"/>.
        /// Takes the navigatable surface of the board and turns it into a graph
        /// </summary>
        /// <returns>graph of navigatable portion of the board</returns>
        public Graph<Cell> convertBoardToGraph()
        {
            Graph<Cell> graph;
            List<int> ids = new List<int>();
            Dictionary<Cell, Dictionary<Cell, double>> adjList = new Dictionary<Cell, Dictionary<Cell, double>>();

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (cells[row, col].cellType.navigatable)
                    {
                        List<Cell> adjCells = getAdjacentCells(row, col);
                        Dictionary<Cell, double> weightedAdjCells = new Dictionary<Cell, double>();

                        foreach(Cell cell in adjCells)
                        {
                            weightedAdjCells.Add(cell, cell.cellType.cost);
                        }

                        adjList.Add(cells[row, col], weightedAdjCells);
                    }
                }
            }

            graph = new Graph<Cell>(adjList);
            

            return graph;
        }

        
        

        /// <summary>
        /// Helper method
        /// Gets the cells adjacent to the <see cref="Cell"/> at the given coordinates
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private List<Cell> getAdjacentCells(int row, int col)
        {
            List<Cell> adjCells = new List<Cell>();

            if (row > 1)
            {
                if(cells[row - 1, col].cellType.navigatable)
                {
                    adjCells.Add(cells[row - 1, col]);
                }

                if (col > 1)
                {
                    if (cells[row - 1, col - 1].cellType.navigatable)
                    {
                        adjCells.Add(cells[row - 1, col - 1]);
                    }
                }
                if (col < size - 1)
                {
                    if (cells[row - 1, col + 1].cellType.navigatable)
                    {
                        adjCells.Add(cells[row - 1, col + 1]);
                    }
                }
            }
            if (row < size - 1)
            {
                if (cells[row + 1, col].cellType.navigatable)
                {
                    adjCells.Add(cells[row + 1, col]);
                }

                if (col > 1)
                {
                    if (cells[row + 1, col - 1].cellType.navigatable)
                    {
                        adjCells.Add(cells[row + 1, col - 1]);
                    }
                }
                if (col < size - 1)
                {
                    if (cells[row + 1, col + 1].cellType.navigatable)
                    {
                        adjCells.Add(cells[row + 1, col + 1]);
                    }
                }
            }
            if (col > 1)
            {
                if (cells[row,col - 1].cellType.navigatable)
                {
                    adjCells.Add(cells[row, col - 1]);
                }
            }
            if (col < size - 1)
            {
                if (cells[row, col + 1].cellType.navigatable)
                {
                    adjCells.Add(cells[row, col + 1]);
                }
            }

            return adjCells;
        }
        
    }

}
