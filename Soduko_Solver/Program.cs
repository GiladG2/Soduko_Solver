using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    internal class Program
    {
        static public void Test(string matBase)
        {
            try
            {
                int[,] mat = Board_Formatter.Format(matBase);
                Sudoku_Solver s = new Sudoku_Solver(mat);
                Console.WriteLine($"Board size : {mat.GetLength(0)} X {mat.GetLength(0)}");
                Console.WriteLine("Original mat");
                Board_Formatter.Print_Mat(mat);
                Stopwatch stopwatch = Stopwatch.StartNew();
                bool verdict = Sudoku_Solver.Solve_Sudoku(mat);
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                Console.WriteLine();
                Console.WriteLine("Solved mat");
                Board_Formatter.Print_Mat(mat);
                Console.WriteLine($"Execution Time: {ts.TotalMilliseconds} ms");
                if (verdict == false || !Tester.TestSolvedSudoku(mat))
                    Console.WriteLine("The board is unsolveable");
                Console.WriteLine();
                Console.WriteLine();
            }
            catch (Invalid_Length_Exception ex)
            {
            }
            catch (Invalid_Character_Exception ex) { }
        }
        static void Main(string[] args)
        {
            string matBase44 = "0003050010040030";
            Test(matBase44);
            string matBase = "800000070006010053040600000000080400003000700020005038000000800004050061900002000";
            string matBase1616 = ";5?000=000000>030000046050100=:0:<80?00000;@20600910503;:0000000800000012@:070;0000@0600>0900:<15:00>0;00=8300201070@0920<0050>000009005060<00?8@0>0=30082?4:00;=;000>0000301600<04060000>01@30203000000=050600000=?79030;428<0:00:405000900=;0>080040<>73@:0200";
            Test(matBase);
            Test(matBase1616);
            Test("005300000800000020070010500400005300010070006003200080000060500000009004000000030");
            Console.WriteLine();
            Console.WriteLine("Write 'End' to exit the solver");
            while (true)
            {
                Console.WriteLine("Enter a mat");
                matBase = Console.ReadLine();
                
                if (matBase == "End")
                    break;
                Test(matBase);
            }
            Console.WriteLine("Thanks for using my Sudoku Solver!");
        }
    }
}
