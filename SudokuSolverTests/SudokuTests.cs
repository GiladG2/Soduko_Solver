using Soduko_Solver;
using System.Diagnostics;
namespace SudokuSolverTests
{
    [TestClass]
    public sealed class SudokuTests
    {
        [TestMethod]
        
        public void Invalid_Sudoku_Len_Test()
        {
            //an 80 characters sudoku board, invalid length 
            string invalid_sudoku = "80000007000601005304060000000008040000300070002000503800000080000405006190000200";
            Assert.ThrowsException<Invalid_Length_Exception>(() => Board_Formatter.Format(invalid_sudoku));
        }

        [TestMethod]
        public void Empty_Board_Handling_Test()
        {
            string invalid_sudoku = "";
            Assert.ThrowsException<Empty_Board_Exception>(() => Board_Formatter.Format(invalid_sudoku));
        }
        [TestMethod]
        public void Duplicate_Value_In_Column_Test()
        {
            //Invalid board, 8 appears twice at column 9
            string invalid_sudoku = "800000070006010053040600000000080400003000700020005038000000800004050061900002008";
            int[,] invalid_mat = Board_Formatter.Format(invalid_sudoku);
            Sudoku_Solver s = new Sudoku_Solver(invalid_mat);
            Assert.ThrowsException<Duplicate_Val_In_Column>(() => Sudoku_Solver.Solve_Sudoku());
        }
        [TestMethod]
        public void Duplicate_Value_In_Box_Test()
        {
            //Invalid board, 5 appears twice at box 5
            string invalid_sudoku = "800000070006010053040600000000580400003000700020005038000000800004050061900002008";
            int[,] invalid_mat = Board_Formatter.Format(invalid_sudoku);
            Sudoku_Solver s = new Sudoku_Solver(invalid_mat);
            Assert.ThrowsException<Duplicate_Val_In_Box>(() => Sudoku_Solver.Solve_Sudoku());
        }
        [TestMethod]
        public void Duplicate_Value_In_Row_Test()
        {
            //Invalid board, 2 appears twice at row 9
            
            Assert.ThrowsException<Duplicate_Val_In_Row>(() => {
                string invalid_sudoku = "800000070006010053040600000000080400003000700020005038000000800004050061900002002";
                int[,] invalid_mat = Board_Formatter.Format(invalid_sudoku);
                Sudoku_Solver s = new Sudoku_Solver(invalid_mat);
                Sudoku_Solver.Solve_Sudoku();
            });
        }
        [TestMethod]
        public void Invalid_Character_In_Board_Test()
        {
            //Invalid board, א is not in the domain of valid symbols
            string invalid_sudoku = "80000007000601005304060000000008040000300070002000א038000000800004050061900002000";
            int[,] invalid_mat = Board_Formatter.Format(invalid_sudoku);
            Sudoku_Solver s = new Sudoku_Solver(invalid_mat);
            Assert.ThrowsException<Invalid_Character_Exception>(() => Sudoku_Solver.Solve_Sudoku());
        }
        [TestMethod]
        //Unsolveable board exception is thrown if TestSolvedSudoku returns false
        public void Unsolveable_Board_Exception_Test()
        {
            //Assume solved mat is the final board returned by my solver (Backtracking() returned false)
            string final_string = "800000070006010053040600000000080400003000700020000038000000800004050061900002000";
            int[,] final_mat = Board_Formatter.Format(final_string);
            Assert.AreEqual(false,Sudoku_Validator.TestSolvedSudoku(final_mat));
        }
        //Tests to see the ability of the solver to solve various sizes
        //if the TestSolvedSudoku returned true, than the board has been solved correctly
        [TestMethod]
        public void Solve_4_4_Test()
        {
            string matBase = "0003040010040030";
            int[,] mat = Board_Formatter.Format(matBase);
            Sudoku_Solver s = new Sudoku_Solver(mat);
            string solvedMatBase = Sudoku_Solver.Solve_Sudoku();
            int[,] solvedMat = Board_Formatter.Format(solvedMatBase);
            Assert.AreEqual(true, Sudoku_Validator.TestSolvedSudoku(solvedMat));
        }
        [TestMethod]
        public void Solve_9_9_Test()
        {
            string matBase = "800000070006010053040600000000080400003000700020005038000000800004050061900002000";
            int[,] mat = Board_Formatter.Format(matBase);
            Stopwatch sw = new Stopwatch();
            Sudoku_Solver s = new Sudoku_Solver(mat);
            sw.Start();
            string solvedMatBase = Sudoku_Solver.Solve_Sudoku();
            sw.Stop();
            int[,] solvedMat = Board_Formatter.Format(solvedMatBase);
            Assert.AreEqual(true, Sudoku_Validator.TestSolvedSudoku(solvedMat));
        }
        [TestMethod]
        public void Solve_16_16_Test()
        {
            string matBase = ";5?000=000000>030000046050100=:0:<80?00000;@20600910503;:0000000800000012@:070;0000@0600>0900:<15:00>0;00=8300201070@0920<0050>000009005060<00?8@0>0=30082?4:00;=;000>0000301600<04060000>01@30203000000=050600000=?79030;428<0:00:405000900=;0>080040<>73@:0200";
            int[,] mat = Board_Formatter.Format(matBase);
            Stopwatch sw = new Stopwatch();
            Sudoku_Solver s = new Sudoku_Solver(mat);
            sw.Start();
            string solvedMatBase = Sudoku_Solver.Solve_Sudoku();
            sw.Stop();
            int[,] solvedMat = Board_Formatter.Format(solvedMatBase);
            Assert.AreEqual(true, Sudoku_Validator.TestSolvedSudoku(solvedMat));
        }
        [TestMethod]
        public void Solve_25_25_Test()
        {
            string matBase = "0E003200000000F000<0000000=00000010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000200010000000000000000000600000000000000009000000401000000001000000000000000000200000000000000000000000500001000000000000000000700000000000000000020000001000000000000000000000000000000000000000000000000000000000000000000010000000000000200000000000000000000000000000000000000000000000000000001000000002000000000010000000020000030000000000000000B000000001000000000000000000000000000000000000000000000000000000000C00000000000000000000000000000000001000000000A000";
            int[,] mat = Board_Formatter.Format(matBase);
            Stopwatch sw = new Stopwatch();
            Sudoku_Solver s = new Sudoku_Solver(mat);
            sw.Start();
            string solvedMatBase = Sudoku_Solver.Solve_Sudoku();
            sw.Stop();
            int[,] solvedMat = Board_Formatter.Format(solvedMatBase);
            Assert.AreEqual(true, Sudoku_Validator.TestSolvedSudoku(solvedMat));
        }
        //Test to check if the solver manages to solve a 9X9 sudoku in under 1 second
        [TestMethod]
        public void Solve_9_9_Time_Limit_Test()
        {
            string matBase = "800000070006010053040600000000080400003000700020005038000000800004050061900002000";
            int[,] mat = Board_Formatter.Format(matBase);
            Stopwatch sw = new Stopwatch();
            Sudoku_Solver s = new Sudoku_Solver(mat);
            sw.Start();
            string solvedMatBase = Sudoku_Solver.Solve_Sudoku();
            sw.Stop();
            int[,] solvedMat = Board_Formatter.Format(solvedMatBase);
            Assert.AreEqual(true, sw.ElapsedMilliseconds < 1000);
        }
        //Test to check if Invalid_Space_Exception is thrown when there is a space in the board the user entered
        [TestMethod]
        public void Space_Exception_Test()
        {
            string invalid_sudoku = " ";
            int[,] invalid_mat = Board_Formatter.Format(invalid_sudoku);
            Sudoku_Solver s = new Sudoku_Solver(invalid_mat);
            Assert.ThrowsException<Invalid_Space_Exception>(() => Sudoku_Solver.Solve_Sudoku());
        }
    }
}
