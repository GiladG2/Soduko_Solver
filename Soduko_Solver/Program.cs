using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
namespace Soduko_Solver
{
    public class Program
    {
        static void Main(string[] args)
        {
            int[] a = { };
            Console.WriteLine(a.Length);
            Console.WriteLine("Welcome to my Sudoku solver");
            Console.WriteLine();
            string matBase = ""; //saves the string of the mat
            Stopwatch sw = new Stopwatch();
            Console.WriteLine("Write 'End' to exit the solver");
            while (true)
            {
                Console.WriteLine("Enter a mat");
                matBase = Console.ReadLine();
                if (matBase == "End")
                    break;
                if(matBase == "Fight")
                {
                    sw.Start();
                    for (int i = 0; i < 500; i++)
                    {
                        Console.WriteLine($"Sudoku num. {i + 1}");
                        string testMat = "831529674796814253542637189159783426483296715627145938365471892274958361918362547";
                        int[,] testMatFormatted = Board_Formatter.Format(testMat);
                        Sudoku_Solver s2 = new Sudoku_Solver(testMatFormatted);
                        testMat = Tester.GenerateSudokuBoard(testMatFormatted, (int)(testMatFormatted.GetLength(0) * testMatFormatted.GetLength(0) * 0.97));
                        Sudoku_Runner.Run(testMat); // Solve the generated hard sudoku
                    }
                    sw.Stop();
                    Console.WriteLine(sw.Elapsed);
                    sw.Reset();
                    continue;
                }
                Sudoku_Runner.Run(matBase); // Solve the Sudoku
            }
            Console.WriteLine("Thanks for using my Sudoku Solver!");
        }
    }
}
