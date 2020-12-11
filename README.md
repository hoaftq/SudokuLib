## SudokuLib
A Sudoku library, which targets .NET Standard 1.0, supports generating game boards

Project SudokuLib contains 2 namespaces whose classes can be used directly depending on your purpose

### 1. Sudoku.Generator namespace
  This is the low level library. It has a generator which can be used to generate new game boards and a solver which can be used to solve game boards with pre-filled values. 
  Both are derived from a generator base GeneratorBase. This is where the backtracking algorithm implemented. You can use this part of the library to develop your own game or a game solver.

  *Usages*
  - **Generator usages**
  
    - Generate a game board
    ```c#
      int x = 3, y = 2;
      var game = new SudokuGenerator(x, y, Processor);
      game.Generate();

      bool Processor(int[][] contents)
      {
          // Here we have a game board stored in contents argument, we can store or process it whatever we want

          // return false if we want to stop generating a new game board, otherwise the generator continues to generate a different game board
          return false;
      }
    ```


    - Create a permutation
    ```c#
      int x = 2, y = 2;
      var game = new SudokuGenerator(x, y, null);
      int[] values = game.GetPermutation();
    ```
    For example, you have the following goard board
    | 1 | 2 | 3 | 4 |
    |---|---|---|---|
    | **3** | **4** | **1** | **2** |
    | **2** | **1** | **4** | **3** |
    | **4** | **3** | **2** | **1** |

    When using SudokuGenerator.GetPermuation you will get an permuation of the set [1, 2, 3, 4], let say [2 4 1 3]  
    That means we will transform our game board with this map  
        1 -> 2  
        2 -> 4  
        3 -> 1  
        4 -> 3  
    Now we have another gameboard 
    | 2 | 4 | 1 | 3 |
    |---|---|---|---|
    | **1** | **3** | **2** | **4** |
    | **4** | **2** | **3** | **1** |
    | **3** | **1** | **4** | **2** |

    In fact, SudokuGenerator.Generate always generates game boards with the first row is a sequence from 1 -> max number. 
    So we have to use a permuation to create a random one.


    - Create a random mark
    ```c#
      int x = 2, y = 2;
      var game = new SudokuGenerator(x, y, null);
      var mask = game.CreateRandomMask(numberOfOpenBoxes);
      // If mask[i][j] is true then the value at coordinate (i, j) is revealed to user
    ```
    If we have a game board like this
    | 2 | 4 | 1 | 3 |
    |---|---|---|---|
    | **1** | **3** | **2** | **4** |
    | **4** | **2** | **3** | **1** |
    | **3** | **1** | **4** | **2** |
  
    and we get the following mask
    | true | true | true | false |
    |---|---|---|---|
    | **false** | **false** | **false** | **false** |
    | **false** | **false** | **true** | **true** |
    | **false** | **false** | **false** | **false** |

    (true means the corresponding value is displayed to user, false means user has to fill it)  
    then you will display the game board to user like this    
    | 2 | 4 | 1 | ? |
    |---|---|---|---|
    | **?** | **?** | **?** | **?** |
    | **?** | **?** | **3** | **1** |
    | **?** | **?** | **?** | **?** |

- **Solver usage**

  Provide a gameboard with missing values denoted with 0, SodokuSolver will find complete game boards for us
  ```c#
    int X = 3, Y = 3;
  int[][] presetBoard =
  {
      new int[]{4, 0, 0,   2, 0, 6,   1, 7, 0},
      new int[]{0, 9, 0,   5, 0, 3,   0, 0, 0},
      new int[]{0, 5, 0,   0, 7, 0,   6, 9, 0},

      new int[]{0, 0, 0,   7, 3, 0,   0, 1, 0},
      new int[]{0, 0, 0,   0, 5, 0,   2, 0, 9},
      new int[]{0, 0, 0,   0, 4, 1,   0, 0, 6},

      new int[]{0, 4, 5,   0, 6, 0,   0, 0, 8},
      new int[]{0, 7, 8,   4, 0, 0,   5, 0, 0},
      new int[]{0, 0, 6,   3, 0, 5,   0, 0, 0}
  };
  var solver = new SudokuSolver(X, Y, Processor);
  solver.InitializeBoard(presetBoard);
  solver.Solve();

  bool Processor(int[][] contents)
  {
      // We have the complete goard board stored in agument contents here

      // return true if we want to find all solutions, otherwise it will stop finding
      return true;
  }
  ```


### 2. SudokuLib.Game namespace
  This is a higher level of the library and it uses Sudoku.Generator. It is like back-end of the game, almost forms a game without a view. 
  If you use these classes there, you just need to implement your own game view.

There is also a complete Sudoku game based on this library SudokuUWP. This game is written using UWP so it can be used on every Windows devices.
