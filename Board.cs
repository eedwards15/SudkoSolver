﻿using System;
using System.Collections.Generic;

namespace SodkoSolverv2
{
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
                {0,0,6,0,4,0,0,7,0},
                {0,0,0,0,0,8,2,0,0},
                {0,9,0,7,0,1,6,5,0},
                {6,1,0,9,0,0,4,2,7},
                {9,0,0,0,1,0,0,0,5},
                {5,8,7,0,0,4,0,1,6},
                {0,5,1,4,0,6,0,3,0},
                {0,0,4,8,0,0,0,0,0},
                {0,2,0,0,5,0,8,0,0}

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


}