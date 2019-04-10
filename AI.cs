using System;
using System.Collections.Generic;
using System.Linq;


namespace SodkoSolverv2
{
    public class AI
    {
        public Squares[,] savedStateGameBoard;
        public Board gameBoard;
        List<int> CanBe = new List<int>() { 1,2,3,4,5,6,7,8,9 };


        public AI(Board board)
        {
            gameBoard = board;
        }



        public void solveBoard()
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

            gameBoard.printBoard();

        }


        private bool Logic(ref bool solved)
        {

            int lowestRow = -1;
            int lowestCol = -1;
            int lowestCount = -1;

            Random random = new Random();


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

            if (lowestRow != -1 && lowestCol != -1)
            {
                var guess = CanBe.Where(item => !gameBoard.GameBoard[lowestRow, lowestCol].cantBe.Contains(item)).ToArray();
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

                gameBoard.SetValue(lowestCol, lowestRow, randomGuess, ChangeType.Update);
            }

            if (lowestCol == -1 && lowestRow == -1 && lowestCount == -1)
            {
                return true; 
            }



            lowestCol = -1;
            lowestRow = -1;
            lowestCount = -1;


            return false; 
        }
    }
}