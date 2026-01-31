using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soduko_Solver
{
    public class Sudoku_Runner
    {
        public static string Run(string matBase)
        {
            string final_mat = matBase;
            try
            {
                int[,] mat = Board_Formatter.Format(matBase); //Format the mat
                Sudoku_Solver s = new Sudoku_Solver(mat); //Set up the solver
                Console.WriteLine($"Board size : {mat.GetLength(0)} X {mat.GetLength(0)}");
                Console.WriteLine("Original mat");
                Board_Formatter.Print_Mat(mat);
                Stopwatch stopwatch = Stopwatch.StartNew(); //Initialize the stopwatch
                final_mat = Sudoku_Solver.Solve_Sudoku(); // Solve
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                Console.WriteLine();
                Console.WriteLine("Solved mat");
                Board_Formatter.Print_Mat(mat);
                Console.WriteLine($"Execution Time: {ts.TotalSeconds} s");
                if (!Tester.TestSolvedSudoku(mat))
                    throw new Unsolvable_Mat_Exception();
                Console.WriteLine($"Solved mat: {final_mat}");
                Console.WriteLine();
                Console.WriteLine();
            }
            catch (Invalid_Length_Exception ex) { }
            catch (Invalid_Character_Exception ex) { }
            catch (Duplicate_Val_In_Box ex) { }
            catch (Unsolvable_Mat_Exception ex) { }
            catch (Duplicate_Val_In_Column ex) { }
            catch (Duplicate_Val_In_Row ex) { }
            catch (Empty_Board_Exception ex) { }
            return final_mat;
        }
    }
}
