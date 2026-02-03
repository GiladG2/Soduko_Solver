using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    //Class to represent a general exception in my solver
    public class Base_Exceptions : Exception
    {
        public Base_Exceptions(string msg)
        {
            Console.WriteLine($"Error : {msg}");
        }
    }
    //Exception when encountered with an invalid sudoku board length
    public class Invalid_Length_Exception: Base_Exceptions
    {
        public Invalid_Length_Exception() : base("Invalid soduko board len")
        {
        }
    }
    //Exception when encountered with an invalid character in the board
    public class Invalid_Character_Exception: Base_Exceptions
    {
        public Invalid_Character_Exception(int val, int i,int j) : base($"Invalid character {(char)(val + '0')} at ({i},{j})")
        {
        }
    }
    //Exception when encountered with an unsolveable mat
    public class Unsolvable_Mat_Exception : Base_Exceptions
    {
        public Unsolvable_Mat_Exception() : base("The board is unsolveable")
        {
        }
    }
    //Exception when the solver found a duplicate value in a column
    public class Duplicate_Val_In_Column: Base_Exceptions
    {
        public Duplicate_Val_In_Column(int val,int col) : base($"Unsolveable board, {val} appears twice at column {col}")
        {
        }
    }
    //Exception when the solver found a duplicate value in a row
    public class Duplicate_Val_In_Row : Base_Exceptions
    {
        public Duplicate_Val_In_Row(int val, int row) : base($"Unsolveable board, {val} appears twice at row {row}")
        {

        }
    }
    //Exception when the solver found a duplicate value in a box
    public class Duplicate_Val_In_Box : Base_Exceptions
    {
        public Duplicate_Val_In_Box(int val, int box) : base($"Unsolveable board, {val} appears twice at box {box}")
        {

        }
    }
    //Exception when the user entered an empty string
    public class Empty_Board_Exception : Base_Exceptions
    {
        public Empty_Board_Exception() : base("Attempt to solve an empty board") { }
    }
    //Exception when the user entered a board with a space
    public class Invalid_Space_Exception : Base_Exceptions
    {
        public Invalid_Space_Exception() : base("Sudoku board cannot have spaces") { }
    }
}
