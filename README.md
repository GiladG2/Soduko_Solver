# Welcome to my sudoku solver
## The problem
Sudoku is a japanese game where given a semi-filled mat, the goal
is to fill the board while not contradicting with the Sudoku's constraints, which are:
in any given row, column, or box, every digit in the Sudoku's domain can appear once.
In the solver, the domain of allowed symbols in the input string (representation of the numer n) is '0' + n.
```sh
1-9 => 1-9
10 => ':' ('0' + 10)
11 = > ';' ('0 + 11)
```
There are examples in the last part of this README.
# The Sudoku solver
The solver uses various algorithms and optimizations in order to solve any Sudoku.
## MRV
The solver implements MRV (Minimum remaining values), an algorithm where the next chosen cell/guess is the cell with the smallest domain of options.
Assume $cell_1$'s domain is $$d_1 = \\{1,2,5,7\\}$$ => 4 options and $cell_2$'s domain is $$d_2 = \\{3,8,9\\}$$ => 3 options
$cell_2$ will be the next cell that is being guessed.
## Degree Heuristic tie-breaker
If 2 cells have the same amount of options, the cell that will be chosen is the cell with the higher weight.
### Weight
If a guess in cell $(r,c)$ has resulted in a failed sudoku (failed recursion branch), the weight of $(r,c)$ will increment by one.
Meaning that a high weight indicates a cell that has been causing failed branches more often than other cells, thus needing to be guessed earlier to
save on the failed branches it could create in other branches. 
## LCV
The solver also implements LCV (Least constraining value). Up until this point, every part of the solver was logical, with no guessing.
In order to make my solver completely logical, I wanted to find a way to not only pick the next cell logicaly, but also what value was going to be placed.
In order to accomplish that, I implemented LCV => the next value that is being placed in the cell is the value that leaves the most amount of options for its peers (the value that constraints other peers the least).
### Example
Assume $cell_1$'s domain is $$d_1 = \\{1,7\\}$$ and its peers $cell_2$ and $cell_3$'s are $$d_2 = \\{1,2,5\\}$$ and $$d_3 = \\{1,5,7\\}$$ respectively. <br/>
Choosing 1 leaves $$d_2 = \\{2,5\\}$$ and $$d_3 = \\{5,7\\}$$, removing 2 options and leaving 4, <br/>
Choosing 7 leaves $$d_2 = \\{1,2,5\\}$$ and $$d_3 = \\{1,5\\}$$, removing 1 option and leaving 5,
thus the solver will place 7 in $cell_1$.
## Bitmask Optimizations
Representing all of the valid digits in $row_i$ , $box_i$ and $column_i$ with an array of binary number (int),
where if the bit at position n is 1 at row/column/box[i], then n is already placed in $row_i$ / $box_i$ / $column_i$ , leading
to O(1) placement, removal and checking if n has already been placed in the board with bitwise operations.
## Simple Backtracking mode
Due to the reliance of MRV on constraints, it fails to solve in a reasonable time an empty sudoku.
Without constraints, MRV won't have a purpose because most cells's domain contains 80% of the possible numbers,
leading to millions of recursion branches and guesses.
In order to address this problem and make MRV more effiecient, I added a simple brute-force backtracking, that is only triggered once at the start 
of the solver if No. of empty cells > 94% of Board Size = No. of filled cells < 6% of Board Size.
Recorded execution times using various precentages (90% - 100%) in order to cultivate that 94% results in the fastest runs. 
If the solver recognizes a need for a brute-force filling of the board, it calls SimpleBactracking() to fill up to 6% of the board, and then lets the MRV algorithm handles
solving the remaining 94%. Now, because the MRV has constraints to work with, it is significantly faster.
## Naked singles chain (Constraint propagation)
Naked singles are cells where their domain $d = 1$.
At the start of each backtracking call, if a board is larger than 9X9, and most of its cells (70%) are filled, 
a naked singles chain is triggered.
Naked singles placements can create another naked singles elsewhere on the board.
Fill a naked single while its filling has created another naked single, chaining multiple forced moves at once (Fill [...] while [...] =>
implemented as do-while) cutting down a lot of recursion branches.
If the chaining resulted in a cell whose domain $d=0$ (no valid options), then Rollback the changes made by the naked singles chain.
### RollBack and Changes stack
Implemented a stack that records every single modification that has been made the board. Upon finding a chain that has resulted in a failed sudoku branch 
(or a guess that has resulted in a failed sudoku branch)
, check how many mofidications have been made during the naked singles chain, and pop the moves from the stack to revert them.

# Testing
Used MS's testing and followed testing guidelines (AAA) in order to create 12 test functions for my solver.
Important note: due to the static implementation of the solver, every test has to be checked seperately (if a user
will try to run all the tests at once, the state of the solver may not match the test case)

# UI and usage
Input manually your board (Of sizes 1X1 up to 25X25) after the Enter a mat prompt, for example
```console
Enter a mat
800000070006010053040600000000080400003000700020005038000000800004050061900002000
```
If a user wants to exit the solver, write 'End'

In order to test the capabilites of the solver, the user can type "Fight"
to see the solver solving 500 hard 9X9 sudokus (generated by my solver 
by taking a solved sudoku and randomly removing 97% of its cells, leaving only 2-4 random cells filled).
## Preview
### Example #1
Board used : 800000070006010053040600000000080400003000700020005038000000800004050061900002000
```console
Board size : 9 X 9
Original mat
-------------------------------
|8  0  0  |0  0  0  |0  7  0  |
|0  0  6  |0  1  0  |0  5  3  |
|0  4  0  |6  0  0  |0  0  0  |
-------------------------------
|0  0  0  |0  8  0  |4  0  0  |
|0  0  3  |0  0  0  |7  0  0  |
|0  2  0  |0  0  5  |0  3  8  |
-------------------------------
|0  0  0  |0  0  0  |8  0  0  |
|0  0  4  |0  5  0  |0  6  1  |
|9  0  0  |0  0  2  |0  0  0  |
-------------------------------

Solved mat
-------------------------------
|8  3  1  |5  2  9  |6  7  4  |
|7  9  6  |8  1  4  |2  5  3  |
|5  4  2  |6  3  7  |1  8  9  |
-------------------------------
|1  5  9  |7  8  3  |4  2  6  |
|4  8  3  |2  9  6  |7  1  5  |
|6  2  7  |1  4  5  |9  3  8  |
-------------------------------
|3  6  5  |4  7  1  |8  9  2  |
|2  7  4  |9  5  8  |3  6  1  |
|9  1  8  |3  6  2  |5  4  7  |
-------------------------------
Execution Time: 0.0001873 s
Solved mat: 831529674796814253542637189159783426483296715627145938365471892274958361918362547
```
### Example #2
Board used : 100000027000304015500170683430962001900007256006810000040600030012043500058001000

```console
Board size : 9 X 9
Original mat
-------------------------------
|1  0  0  |0  0  0  |0  2  7  |
|0  0  0  |3  0  4  |0  1  5  |
|5  0  0  |1  7  0  |6  8  3  |
-------------------------------
|4  3  0  |9  6  2  |0  0  1  |
|9  0  0  |0  0  7  |2  5  6  |
|0  0  6  |8  1  0  |0  0  0  |
-------------------------------
|0  4  0  |6  0  0  |0  3  0  |
|0  1  2  |0  4  3  |5  0  0  |
|0  5  8  |0  0  1  |0  0  0  |
-------------------------------

Solved mat
-------------------------------
|1  9  3  |5  8  6  |4  2  7  |
|8  6  7  |3  2  4  |9  1  5  |
|5  2  4  |1  7  9  |6  8  3  |
-------------------------------
|4  3  5  |9  6  2  |8  7  1  |
|9  8  1  |4  3  7  |2  5  6  |
|2  7  6  |8  1  5  |3  4  9  |
-------------------------------
|7  4  9  |6  5  8  |1  3  2  |
|6  1  2  |7  4  3  |5  9  8  |
|3  5  8  |2  9  1  |7  6  4  |
-------------------------------
Execution Time: 5.14E-05 s
Solved mat: 193586427867324915524179683435962871981437256276815349749658132612743598358291764
```
