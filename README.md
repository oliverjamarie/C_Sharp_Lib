# C_Sharp_Lib
Data Structure Library for C#

Known Bugs:
- Dijkstra throwing NullReference exceptions when converting INode<T> to Dijkstra.Node.
Most likely something to do with how Board.Cell and Graph.Node have conflicting ID systems.

Design Choices:
- GameBoard is designed to be flexible and to be easily adapatable to different games.
To make 

Functionalities left to implement:
- MiniMax & MiniMaxAlphaBeta


Potential Future Changes:
- Change Graph's  implementation of ICompareable to ICompareable<Graph>
- Add IEquatable<Graph> implementation to Graph
- Make Move an abstract class to force implementation of score generation
