using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //800000070006010053040600000000080400003000700020005038000000800004050061900002000
            //0000000000000:0000000000;0000000<000=000>000?000@000010002000300040005000600070008000900:000;000<000=000>000?000@000100020003000400050006000700080009000:000;000<000=000>000?000@000100020003000400050006000700080009000:000;000<000=000>000?000@000100020003000400050006000700080009000


            while (true) {
                Console.WriteLine("Enter a soduko board");
                string matBase = Console.ReadLine();
                Board_Formatter board_Formatter = new Board_Formatter();
                Sudoku_Solver solver = new Sudoku_Solver();
                int[,] mat = board_Formatter.Format(matBase);
                solver.Solve_Sudoku(mat);
                board_Formatter.Print_Mat(mat);

            }
        }
    }
}
