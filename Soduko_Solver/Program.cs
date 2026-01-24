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
                string final_mat = Sudoku_Solver.Solve_Sudoku(mat);
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
            catch (Unsolvable_Mat_Exception ex) { }
            catch (Duplicate_Val_In_Box ex) { }
            catch (Duplicate_Val_In_Column ex) { }
            catch (Duplicate_Val_In_Row ex) { }
            catch (Empty_Board_Exception ex) { }
        }
        static void Main(string[] args)
        {
            string matBase44 = "0003040010040030";
            string matBase = "800000070006010053040600000000080400003000700020005038000000800004050061900002000";
            string matBase1616 = ";5?000=000000>030000046050100=:0:<80?00000;@20600910503;:0000000800000012@:070;0000@0600>0900:<15:00>0;00=8300201070@0920<0050>000009005060<00?8@0>0=30082?4:00;=;000>0000301600<04060000>01@30203000000=050600000=?79030;428<0:00:405000900=;0>080040<>73@:0200";
            string matBase2525 = "0E003000000000F000<0000000=00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000600000000000000009000000400000000000000000000000000000000000000000000000000000500000000000000000000000700000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000B000000000000000000000000000000000000000000000000000000000000000000C00000000000000000000000000000000000000000000A000";
            string hardMat = "1020000300000000000000000000000000000000000000000003000020000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000200000000100000000000000300000000000000000000000004000010000000000000000001000000002000000000000000000000000000100000000000000000000000000000000002000000000010000000000000000000000000000000000000000000100000000000002000000000020000000000000000000000000001020000000000000001000000000000000020000000000000000000000000001200000000000000000000000000000100000000000000000000000000000000000010000000004000000000000000012000003000000010000000000000200000000000000000000000000000";
            string matError = "005300000800000020070010500400005300010070006003200080000060500000009004000000030";
            string[] testBoards = { matBase44, matBase, matBase1616,matBase2525,hardMat};
            for (int i = 0; i < testBoards.Length; i++)
                Test(testBoards[i]);
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
