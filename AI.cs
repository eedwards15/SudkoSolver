using System;
using System.Collections.Generic;
using System.Linq;


namespace SodkoSolverv2
{
    public class AI
    {
        public Board gameBoard;
        List<int> CanBe = new List<int>() { 1,2,3,4,5,6,7,8,9 };


        public AI(Board board)
        {
            gameBoard = board;
        }



        public void SolveBoard()
        {

            bool solved = false;

            while (!solved)
            {
                try
                {
                    solved = Logic(ref solved);
                }
                catch (Exception ex)
                {
                    //reset Board. 
                    gameBoard = new Board();
                }
            }

            gameBoard.PrintBoard();

        }


        private bool Logic(ref bool solved)
        {
            Random random = new Random();

            var results = FindSquareWithFewestOptions();
            var row = results.Item1;
            var col = results.Item2;
            var count = results.Item3; 

            if (row != -1 && col != -1)
            {
                GuessValueForColAndRom(random, row, col);
            }

            if (col == -1 && row == -1 && count == -1)
            {
                return true;
            }


            return false;
        }

        private void GuessValueForColAndRom(Random random, int row, int col)
        {
            var guess = CanBe.Where(item => !gameBoard.GameBoard[row, col].cantBe.Contains(item)).ToArray();
            int randomGuess;

            if (guess.Count() > 1)
            {
                int index = random.Next(1, guess.Count() - 1);
                randomGuess = guess[index];
            }
            else
            {
                randomGuess = guess.First();
            }

            gameBoard.SetValue(col, row, randomGuess, ChangeType.Update);
        }

        private Tuple<int,int,int> FindSquareWithFewestOptions()
        {

            int lowestRow = -1;
            int lowestCol = -1;
            int lowestCount = -1;


            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (gameBoard.GameBoard[row, col].squareValue == 0)
                    {
                        var missing = CanBe.Where(item => !gameBoard.GameBoard[row, col].cantBe.Distinct().ToList().Contains(item)).ToList();

                        if (lowestCount == -1 || lowestCount > missing.Count())
                        {
                            lowestRow = row;
                            lowestCol = col;
                            lowestCount = missing.Count();
                        }
                    }
                }
            }

            return new Tuple<int, int, int>(lowestRow, lowestCol, lowestCount); 

        }
    
    
    
    }
}