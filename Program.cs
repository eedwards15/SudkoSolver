using System;
using System.Collections.Generic;
using System.Linq;
using SodkoSolverv2; 

//http://www.sudokupuzz.com/how-to.html
namespace SodkoSolverv2
{
    class Program
    {
        static void Main(string[] args)
        {
            AI aI = new AI(new Board()); 
             aI.solveBoard();
            aI.gameBoard.printBoard();
        }
    }

}
