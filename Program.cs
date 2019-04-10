using System;
using System.Collections.Generic;
using System.Linq;

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

    public class Squares
    {
        public int squareValue { get; set; }
        public List<int> cantBe { get; set; }
        public bool cantChange { get; set; }
    }

    public class Board
    {
        public Squares[,] GameBoard;

        public enum ChangeType
        {
            Update = 0,
            FirstSet = 1,
            BoardSetup = 2
        }

        public Board()
        {
            GameBoard = new Squares[9, 9];
            init();
            SetupInitValues();
        }

        private void init()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    GameBoard[row, col] = new Squares() { cantBe = new List<int>(), squareValue = 0, cantChange = false };
                }
            }
        }

        public void printBoard()
        {
            for (int row = 0; row < 9; row++)
            {
                Console.WriteLine(" ");
                for (int col = 0; col < 9; col++)
                {
                    Console.Write("|  " + GameBoard[row, col].squareValue + "  ");
                }
            }
            Console.WriteLine();
        }

        public void SetupInitValues()
        {
            int[,] puzzle = new int[9, 9] {
                { 8, 5, 0, 0, 9, 0, 4, 0,7},
                {9, 0, 0, 5, 0, 0, 0, 0, 0},
                {0, 6, 0, 0, 0, 7, 9, 0, 5},
                {0, 2, 0, 1, 0, 0, 0, 4, 0},
                {7, 9, 8, 0, 0, 0, 1, 2, 3},
                {0, 4, 0, 0, 0, 2, 0, 5, 0},
                {4, 0, 2, 7, 0, 0, 0, 9, 0},
                {0, 0, 0, 0, 0, 1, 0, 0, 8},
                {1, 0, 5, 0, 2, 0, 0, 7, 4}

            };


            for (int row = 0; row < 9; row++)
            {

                for (int col = 0; col < 9; col++)
                {
                    if (puzzle[row, col] != 0)
                    {
                        SetValue(col, row, puzzle[row, col], ChangeType.BoardSetup);
                    }
                }
            }


            printBoard(); 
          
        }

        public bool checkSquares(int gridSection, int rowSection, int value)
        {
            int section = (gridSection * 3);
            int row_Section = rowSection * 3;

            for (int x = (rowSection - 1) * 3; x < row_Section; x++)
            {
                for (int y = (gridSection - 1) * 3; y < section; y++)
                {
                    if (GameBoard[x, y].squareValue == value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool miniGridUpdate(int colsection, int rowSection, int value)
        {
            int section = (colsection * 3);
            int row_Section = rowSection * 3;

            for (int x = (rowSection - 1) * 3; x < row_Section; x++)
            {
                for (int y = (colsection - 1) * 3; y < section; y++)
                {
                    GameBoard[x, y].cantBe.Add(value); 

                }
            }

            return false;
        }

        public bool SetValue(int col, int row, int value, ChangeType change)
        {
            if (GameBoard[row, col].cantChange) return false;

            if (change == ChangeType.BoardSetup)
            {
                GameBoard[row, col].cantChange = true;
            }

            if (GameBoard[row, col].cantBe.Contains(value) == false)
            {
                int colSection = GetSections(col);
                int rowSection = GetSections(row);

                if (checkSquares(colSection, rowSection, value) == false)
                {
                    miniGridUpdate(colSection, rowSection, value); 
                    Update(value, row, col, ChangeType.Update);
                    GameBoard[row, col].squareValue = value;
                    return true; 
                }
            }





            return false; 

        }

        private static int GetSections(int col)
        {
            if (col <= 2)
                return 1;

            if (col >= 3 && col < 6)
                return 2;

            //else 
            return 3;
        }

        private void Update(int value, int row, int col, ChangeType changeType)
        {
            UpdateRow(value, row, col, changeType);

            UpdateColumn(value, row, col, changeType);
        }

        private void UpdateColumn(int value, int row, int col, ChangeType changeType)
        {


            for (int column = 0; column < 9; column++)
            {
                GameBoard[row, column].cantBe.Add(value);
            }

        }

        private void UpdateRow(int value, int row, int col, ChangeType changeType)
        {
            for (int _row = 0; _row < 9; _row++)
            {
                GameBoard[_row, col].cantBe.Add(value);
            }
        }


    }

    public class rowInfo{
        public int row { get; set; }
        public int rowCount { get; set; }
    }


    public class AI
    {
        public Squares[,] savedStateGameBoard;

        List<int> CanBe = new List<int>()
        {
            1,2,3,4,5,6,7,8,9
        };


        public Board gameBoard;

        public AI(Board board)
        {
            gameBoard = board;
        }



        public void solveBoard()
        {
            Random random = new Random();
            bool rowComplete = true;
            bool notSolved = true;
            ///fillGrid(random, rowComplete); 
            fillGridV3();  



        }

        private void fillGridV3()
        {
            bool solved = false;

            int lowestRow = -1;
            int lowestCol = -1;
            int lowestCount = -1;

            Random random = new Random(); 


            while (!solved)
            {
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
                    var guess = CanBe.Where(item => !gameBoard.GameBoard[lowestRow,lowestCol].cantBe.Contains(item)).ToArray();
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

                    gameBoard.SetValue(lowestCol, lowestRow, randomGuess , Board.ChangeType.Update);
                }

                if (lowestCol == -1 && lowestRow == -1 && lowestCount == -1)
                {
                    solved = true;
                    gameBoard.printBoard();
                }



                lowestCol = -1;
                lowestRow = -1;
                lowestCount = -1;




            

            }






        }

        private void fillGridv2(){
            List<rowInfo> rowInfos = new List<rowInfo>();

            for (int row = 0; row < 9; row++)
            {
                int col_count = 0; 
                for (int col = 0; col < 9; col++)
                {

                    if(gameBoard.GameBoard[row, col].squareValue == 0)
                    {
                        col_count = col_count + 1; 
                    }



                }

                rowInfos.Add(new rowInfo() { row = row, rowCount = col_count }); 
            }


            //var rowsOrdered = rowInfos.OrderBy(x => x.rowCount).Select(x => x.row).ToList();

            var rowsOrdered = new List<int>() { 5, 0, 1, 2, 4, 6, 8, 7, 3 };


            foreach (var row in rowsOrdered)
            {
                for (int col = 0; col < 9; col++)
                {

                    var missing = CanBe.Where(item => gameBoard.GameBoard[col, row].cantBe.Contains(item)).ToList();
                    int foundValue = -1;

                    while (foundValue == -1)
                    {
                        foreach (var guess in missing)
                        {
                            if (foundValue == -1)
                            {
                                if (gameBoard.SetValue(col, row, guess, Board.ChangeType.Update))
                                {
                                    foundValue = 1;
                                }
                            }
                        }

                        foundValue = 0;
                    }

                }

            }






        }


        private bool fillGrid(Random random, bool rowComplete)
        {

            List<bool> results = new List<bool>(); 

            for (int row = 0; row < 9; row++)
            {
                SaveState();

                for (int col = 0; col < 9; col++)
                {

                    var missing = CanBe.Where(item => gameBoard.GameBoard[col, row].cantBe.Contains(item)).ToList();
                    int foundValue = -1;

                    while (foundValue == -1)
                    {
                        foreach (var guess in missing)
                        {
                            if (foundValue == -1)
                            {
                                if (gameBoard.SetValue(col, row, guess, Board.ChangeType.Update))
                                {
                                    foundValue = 1;
                                }
                            }
                        }

                        if (foundValue == 1)
                        {
                           results.Add(true);
                        }
                        else
                        {
                             results.Add(false);
                        }

                        foundValue = 0;

                    }

                }

                if (rowComplete == false)
                {
                    gameBoard.GameBoard = savedStateGameBoard;
                }

            }

            return !results.Contains(false); 
        }

        public void SaveState() {
            savedStateGameBoard = (Squares[,])gameBoard.GameBoard.Clone();
        }


    }


}
