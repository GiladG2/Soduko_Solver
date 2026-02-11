# Welcome to my sudoku solver
## The problem
Sudoku is a japanese game where given a semi-filled mat, the goal
is to fill the board while not contradicting with the Sudoku's constraints, which are:
in any given row, column, or box, every digit in the Sudoku's domain can appear once.
In the solver, the domain of allowed symbols in the input string (representation of the numer n) is '0' + n where 0 is an empty cell.
```sh
'1-9' => 1-9
```
# The Sudoku solver
The solver uses various algorithms and optimizations in order to solve any Sudoku.
### Important disclaimer 
The solver has been designed to solve larger, 16 X 16 and 25 X 25, boards and also 1 X 1 and 4 X 4.
Many boards that aren't 9 X 9 have been solved and tested (Example in the last part of this README for the full potential of the solver), but for the release of this solver, the board size have been limited to 9 X 9.

## Backtracking
The core of the solver's algorithm is Backtracking.
Via recursion a value is being placed in a cell until either all cells are filled (the Sudoku has been solved) or 
until the Sudoku is unsolveable.
If the Sudoku has been found to be unsolveable, the solver will return and go back via recursion to the last valid state of the Sudoku board, and try to place another value in the cell.
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
Input manually your board (Of size 9 X 9) after the Enter a mat prompt, for example
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

## Preview for larger boards
### Domain of valid symblos
As written in the first part of this README, the formula for valid symbols (for number n) in the solver is '0' + n.
```sh
1-9 => 1-9
':' => 10 ('0' + 10)
';' => 11 ('0' + 11)
And so on
```
### Example of 16 X 16
Board used = ;5?000=000000>030000046050100=:0:<80?00000;@20600910503;:0000000800000012@:070;0000@0600>0900:<15:00>0;00=8300201070@0920<0050>000009005060<00?8@0>0=30082?4:00;=;000>0000301600<04060000>01@30203000000=050600000=?79030;428<0:00:405000900=;0>080040<>73@:0200
```console
Board size : 16 X 16
Original mat
-----------------------------------------------------
|11 5  15 0  |0  0  13 0  |0  0  0  0  |0  14 0  3  |
|0  0  0  0  |0  4  6  0  |5  0  1  0  |0  13 10 0  |
|10 12 8  0  |15 0  0  0  |0  0  11 16 |2  0  6  0  |
|0  9  1  0  |5  0  3  11 |10 0  0  0  |0  0  0  0  |
-----------------------------------------------------
|8  0  0  0  |0  0  0  1  |2  16 10 0  |7  0  11 0  |
|0  0  0  16 |0  6  0  0  |14 0  9  0  |0  10 12 1  |
|5  10 0  0  |14 0  11 0  |0  13 8  3  |0  0  2  0  |
|1  0  7  0  |16 0  9  2  |0  12 0  0  |5  0  14 0  |
-----------------------------------------------------
|0  0  0  0  |9  0  0  5  |0  6  0  12 |0  0  15 8  |
|16 0  14 0  |13 3  0  0  |8  2  15 4  |10 0  0  11 |
|13 11 0  0  |0  14 0  0  |0  0  3  0  |1  6  0  0  |
|12 0  4  0  |6  0  0  0  |0  14 0  1  |16 3  0  2  |
-----------------------------------------------------
|0  3  0  0  |0  0  0  0  |13 0  5  0  |6  0  0  0  |
|0  0  13 15 |7  9  0  3  |0  11 4  2  |8  12 0  10 |
|0  0  10 4  |0  5  0  0  |0  9  0  0  |13 11 0  14 |
|0  8  0  0  |4  0  12 14 |7  3  16 10 |0  2  0  0  |
-----------------------------------------------------

Solved mat
-----------------------------------------------------
|11 5  15 7  |10 1  13 16 |12 8  2  6  |9  14 4  3  |
|3  14 16 2  |12 4  6  8  |5  15 1  9  |11 13 10 7  |
|10 12 8  13 |15 7  14 9  |3  4  11 16 |2  1  6  5  |
|4  9  1  6  |5  2  3  11 |10 7  14 13 |12 15 8  16 |
-----------------------------------------------------
|8  4  6  14 |3  12 5  1  |2  16 10 15 |7  9  11 13 |
|15 2  11 16 |8  6  4  13 |14 5  9  7  |3  10 12 1  |
|5  10 12 9  |14 15 11 7  |1  13 8  3  |4  16 2  6  |
|1  13 7  3  |16 10 9  2  |4  12 6  11 |5  8  14 15 |
-----------------------------------------------------
|2  7  3  10 |9  16 1  5  |11 6  13 12 |14 4  15 8  |
|16 6  14 1  |13 3  7  12 |8  2  15 4  |10 5  9  11 |
|13 11 9  8  |2  14 15 4  |16 10 3  5  |1  6  7  12 |
|12 15 4  5  |6  11 8  10 |9  14 7  1  |16 3  13 2  |
-----------------------------------------------------
|9  3  2  12 |11 8  10 15 |13 1  5  14 |6  7  16 4  |
|14 1  13 15 |7  9  16 3  |6  11 4  2  |8  12 5  10 |
|7  16 10 4  |1  5  2  6  |15 9  12 8  |13 11 3  14 |
|6  8  5  11 |4  13 12 14 |7  3  16 10 |15 2  1  9  |
-----------------------------------------------------
Execution Time: 0.0052407 s
Solved mat: ;5?7:1=@<8269>433>@2<4685?19;=:7:<8=?7>934;@21654916523;:7>=<?8@846>3<512@:?79;=?2;@864=>5973:<15:<9>?;71=834@261=73@:924<6;58>?273:9@15;6=<>4?8@6>1=37<82?4:59;=;982>?4@:35167<<?456;8:9>71@3=2932<;8:?=15>67@4>1=?79@36;428<5:7@:41526?9<8=;3>685;4=<>73@:?219
```
### Example of 25 X 25 
Board used = 0E003200000000F000<0000000=00000010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000200010000000000000000000600000000000000009000000401000000001000000000000000000200000000000000000000000500001000000000000000000700000000000000000020000001000000000000000000000000000000000000000000000000000000000000000000010000000000000200000000000000000000000000000000000000000000000000000001000000002000000000010000000020000030000000000000000B000000001000000000000000000000000000000000000000000000000000000000C00000000000000000000000000000000001000000000A000
```console
Board size : 25 X 25
Original mat
---------------------------------------------------------------------------------
|0  21 0  0  3  |2  0  0  0  0  |0  0  0  0  22 |0  0  0  12 0  |0  0  0  0  0  |
|0  13 0  0  0  |0  0  0  1  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |
|0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |
|0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |
|0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |
---------------------------------------------------------------------------------
|0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |
|0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  2  0  0  |0  1  0  0  0  |
|0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  6  0  0  0  |0  0  0  0  0  |
|0  0  0  0  0  |0  0  0  9  0  |0  0  0  0  0  |4  0  1  0  0  |0  0  0  0  0  |
|0  1  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |2  0  0  0  0  |
---------------------------------------------------------------------------------
|0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  5  |0  0  0  0  1  |
|0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  7  0  |0  0  0  0  0  |
|0  0  0  0  0  |0  0  0  0  0  |0  0  2  0  0  |0  0  0  0  1  |0  0  0  0  0  |
|0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |
|0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |
---------------------------------------------------------------------------------
|0  0  0  0  0  |0  0  0  0  0  |0  0  1  0  0  |0  0  0  0  0  |0  0  0  0  0  |
|0  2  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |
|0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |
|0  0  0  0  0  |0  0  1  0  0  |0  0  0  0  0  |0  2  0  0  0  |0  0  0  0  0  |
|0  0  1  0  0  |0  0  0  0  0  |0  2  0  0  0  |0  0  3  0  0  |0  0  0  0  0  |
---------------------------------------------------------------------------------
|0  0  0  0  0  |0  0  0  0  18 |0  0  0  0  0  |0  0  0  1  0  |0  0  0  0  0  |
|0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |
|0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |
|0  19 0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |0  0  0  0  0  |
|0  0  0  0  0  |0  0  0  0  0  |0  1  0  0  0  |0  0  0  0  0  |0  17 0  0  0  |
---------------------------------------------------------------------------------

Solved mat
---------------------------------------------------------------------------------
|1  21 4  5  3  |2  6  7  8  9  |17 18 19 10 22 |11 14 15 12 16 |20 13 23 24 25 |
|9  13 8  10 11 |18 21 19 1  17 |23 4  3  24 25 |2  5  7  6  20 |12 14 15 16 22 |
|6  17 7  12 16 |22 23 24 25 15 |20 5  11 13 14 |19 9  4  3  18 |1  2  8  10 21 |
|2  20 22 24 25 |5  11 12 13 14 |6  7  15 16 21 |1  23 17 8  10 |19 3  4  9  18 |
|19 18 23 14 15 |3  4  10 16 20 |1  9  8  2  12 |24 25 21 13 22 |17 5  6  7  11 |
---------------------------------------------------------------------------------
|3  9  19 20 4  |15 1  6  2  8  |16 11 13 21 24 |10 12 5  14 7  |22 23 18 25 17 |
|18 10 24 23 6  |17 13 4  7  5  |22 25 12 20 8  |15 11 2  16 9  |21 1  14 3  19 |
|13 5  12 17 2  |23 18 22 14 25 |3  15 10 1  4  |20 6  24 21 19 |16 11 9  8  7  |
|8  7  11 25 21 |10 12 16 9  19 |14 23 18 5  2  |4  17 1  22 3  |6  24 13 15 20 |
|15 1  16 22 14 |11 3  20 21 24 |9  6  7  19 17 |23 13 18 25 8  |2  4  10 5  12 |
---------------------------------------------------------------------------------
|25 14 17 4  23 |8  22 13 10 16 |15 12 6  9  20 |3  24 19 2  5  |7  18 11 21 1  |
|12 8  2  11 1  |9  19 23 17 6  |25 22 16 4  5  |18 21 10 7  15 |13 20 24 14 3  |
|10 22 6  18 24 |14 7  3  5  4  |19 21 2  11 13 |9  16 20 17 1  |23 15 25 12 8  |
|21 3  9  13 19 |20 25 18 15 11 |10 24 17 7  1  |12 8  14 4  23 |5  6  22 2  16 |
|7  15 5  16 20 |24 2  21 12 1  |18 8  14 3  23 |13 22 25 11 6  |9  19 17 4  10 |
---------------------------------------------------------------------------------
|14 24 3  8  7  |6  15 11 18 21 |13 19 1  12 16 |5  4  23 9  17 |10 25 20 22 2  |
|4  2  18 19 9  |7  5  25 23 22 |8  14 20 17 11 |16 1  6  10 12 |15 21 3  13 24 |
|17 11 20 6  5  |16 24 9  3  2  |21 10 4  25 15 |22 7  8  19 13 |14 12 1  18 23 |
|22 16 25 15 13 |12 8  1  20 10 |24 3  23 6  9  |21 2  11 18 14 |4  7  19 17 5  |
|23 12 1  21 10 |19 14 17 4  13 |5  2  22 18 7  |25 15 3  20 24 |11 8  16 6  9  |
---------------------------------------------------------------------------------
|24 23 13 3  12 |25 17 15 6  18 |11 16 5  14 10 |7  19 9  1  2  |8  22 21 20 4  |
|20 6  21 9  17 |1  16 2  19 23 |7  13 24 22 3  |8  18 12 15 4  |25 10 5  11 14 |
|5  4  10 1  18 |21 9  14 22 7  |2  20 25 8  6  |17 3  13 23 11 |24 16 12 19 15 |
|11 19 15 7  22 |13 20 8  24 12 |4  17 21 23 18 |14 10 16 5  25 |3  9  2  1  6  |
|16 25 14 2  8  |4  10 5  11 3  |12 1  9  15 19 |6  20 22 24 21 |18 17 7  23 13 |
---------------------------------------------------------------------------------
Execution Time: 0.0185099 s
Solved mat: 1E45326789ABC:F;>?<@D=GHI9=8:;BEC1AG43HI2576D<>?@F6A7<@FGHI?D5;=>C943B128:E2DFHI5;<=>67?@E1GA8:C349BCBG>?34:@D1982<HIE=FA567;39CD4?1628@;=EH:<5>7FGBIAB:HG6A=475FI<D8?;2@9E1>3C=5<A2GBF>I3?:14D6HEC@;98787;IE:<@9C>GB524A1F36H=?D?1@F>;3DEH967CAG=BI824:5<I>A4G8F=:@?<69D3HC257B;E1<82;19CGA6IF@45BE:7?=DH>3:F6BH>7354CE2;=9@DA1G?I<8E39=CDIB?;:HA71<8>4G56F2@7?5@DH2E<1B8>3G=FI;69CA4:>H3876?;BE=C1<@54G9A:IDF242BC975IGF8>DA;@16:<?E3=HA;D65@H932E:4I?F78C=><1BGF@I?=<81D:H3G69E2;B>47CA5G<1E:C>A4=52FB7I?3DH;8@69HG=3<IA?6B;@5>:7C9128FED4D6E9A1@2CG7=HF38B<?4I:5;>54:1BE9>F72DI86A3=G;H@<C?;C?7F=D8H<4AEGB>:@5I39216@I>284:5;3<19?C6DFHEBA7G=
```
