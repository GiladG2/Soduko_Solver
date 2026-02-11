using Soduko_Solver;
using System.Diagnostics;
namespace SudokuSolverTests
{
    [TestClass]
    public sealed class SudokuTests
    {
        [TestMethod]
        
        public void Invalid_Sudoku_Len_Test1()
        {
            //an 80 characters sudoku board, invalid length 
            string invalid_sudoku = "80000007000601005304060000000008040000300070002000503800000080000405006190000200";
            Assert.ThrowsException<Invalid_Length_Exception>(() => Board_Formatter.Format(invalid_sudoku));
        }
        [TestMethod]

        public void Invalid_Sudoku_Len_Test2()
        {   //an 82 characters sudoku board, invalid length
            string invalid_sudoku = "8000000700060100530406000000000804000030007000200050380000008000040500612290000200";
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
        public void Duplicate_Value_In_Column_Test2()
        {
            //Invalid board, 2 appears twice at column 6
            string invalid_sudoku = "800000070006012053040600000000080400003000700020005038000000800004050061900002008";
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
        public void Duplicate_Value_In_Box_Test2()
        {
            //Invalid board, 6 appears twice at box 9
            string invalid_sudoku = "800000070006010053040600000000080400003000700020005038000000800004050061900002006";
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
        public void Duplicate_Value_In_Row_Test2()
        {
            //Invalid board, 7 appears twice at row 5

            Assert.ThrowsException<Duplicate_Val_In_Row>(() => {
                string invalid_sudoku = "800000070006010053040600000000080400003700700020005038000000800004050061900002000";
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
        public void Invalid_Character_In_Board_Test2()
        {
            //Invalid board, : (10)9 is not in the domain of valid symbols
            string invalid_sudoku = "8000000700060100530406000000000804000030:0700020000038000000800004050061900002000";
            int[,] invalid_mat = Board_Formatter.Format(invalid_sudoku);
            Sudoku_Solver s = new Sudoku_Solver(invalid_mat);
            Assert.ThrowsException<Invalid_Character_Exception>(() => Sudoku_Solver.Solve_Sudoku());
        }
        [TestMethod]
        //Test to check correct throwing of unsolveable board exception
        public void Unsolveable_Board_Exception_Test()
        {
            //Unsolveable board => the cell at row 4 column 6 has no valid options.
            string final_string = "200900000000000060000001000502600407000004100000098023000003080005010000007000000";
            int[,] final_mat = Board_Formatter.Format(final_string);
            Sudoku_Solver s = new Sudoku_Solver(final_mat);
            Assert.ThrowsException<Unsolvable_Mat_Exception>(() => Sudoku_Solver.Solve_Sudoku());
        }
        [TestMethod]
        //Test to check correct throwing of unsolveable board exception
        public void Unsolveable_Board_Exception_Test2()
        {
            //Unsolveable board => the cell at row 4 column 7 has no valid options.
            string final_string = "000000800000000000000000500200300070000000040000000109000000600000000000000000000";
            int[,] final_mat = Board_Formatter.Format(final_string);
            Sudoku_Solver s = new Sudoku_Solver(final_mat);
            Assert.ThrowsException<Unsolvable_Mat_Exception>(() => Sudoku_Solver.Solve_Sudoku());
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
            Assert.AreEqual(true, SudokuSolverTests.Sudoku_Validator.TestSolvedSudoku(solvedMat));
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
            string invalid_sudoku = "80000007000601005304060000000008040000300070002000503800000080000405 061900002000";
            int[,] invalid_mat = Board_Formatter.Format(invalid_sudoku);
            Sudoku_Solver s = new Sudoku_Solver(invalid_mat);
            Assert.ThrowsException<Invalid_Space_Exception>(() => Sudoku_Solver.Solve_Sudoku());
        }
        
    }
}
