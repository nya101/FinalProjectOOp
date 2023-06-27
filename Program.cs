using System;

namespace ConnectFour
{
    // class represents a  players name  AND the game's chip/ abstract class
    public abstract class Player
    {
        public string PlayersName { get; set; }
        public string GameChip { get; set; }

        public Player(string name, string chip)
        {
            PlayersName = name;
            GameChip = chip;
        }

        // method for choosing players choosing a move /abstract Class
        public abstract int[] ChooseMove(ConnectFourGame game);
    }

    // this  class represent a human player (player 1)
    public class HumanPlayer : Player
    {
        public HumanPlayer(string name, string chip) : base(name, chip)
        {
        }

        // implements  the ChooseMove method for (player 1)
        public override int[] ChooseMove(ConnectFourGame game)
        {
            Console.WriteLine(PlayersName + ", it's your turn to play. Please choose a column from  the letters A to G: "); //7 letters for column
            string input = Console.ReadLine();
            return game.CoordinateParser(input);
        }
    }

    // represents the  computer player(player 2)
    public class HumanPlayer2 : Player
    {
        private Random random;

        public HumanPlayer2(string name, string chip) : base(name, chip)
        {
            random = new Random();
        }

        // implements  the ChooseMove method for player 2
        public override int[] ChooseMove(ConnectFourGame game)
        {
            string[] possibleLetters = { "A", "B", "C", "D", "E", "F", "G" };
            string randomLetter = possibleLetters[random.Next(possibleLetters.Length)];
            int randomNumber = random.Next(game.Rows);
            string cpuChoice = randomLetter + randomNumber;
            return game.CoordinateParser(cpuChoice);
        }
    }

    //  an interface that demonstrates any game
    public interface IGame
    {
        void InitializeGameBoard();
        void PrintGameBoard();
        bool MakeMove(Player player);
        bool CheckForWinner(Player player);
    }

    // inteface represents the Connect Four game
    public class ConnectFourGame : IGame
    {
        private string[,] gameBoard;
        public int Rows { get; private set; } = 6;
        public int Cols { get; private set; } = 7;
        public string[] PossibleLetters { get; } = { "A", "B", "C", "D", "E", "F", "G" };

        public ConnectFourGame()
        {
            gameBoard = new string[Rows, Cols];
            InitializeGameBoard();
        }

        // intialize's the board  
        public void InitializeGameBoard()
        {
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Cols; y++)
                {
                    gameBoard[x, y] = "";
                }
            }
        }

        // print board of the game 
        public void PrintGameBoard()
        {
            Console.WriteLine("\n     A    B    C    D    E    F    G  ");//  7 letter;s for the  columns 
            for (int x = 0; x < Rows; x++)
            {
                Console.WriteLine("\n   +---+----+----+----+----+----+----+");
                Console.Write(x + " |");
                for (int y = 0; y < Cols; y++)
                {
                    string chip = gameBoard[x, y];
                    Console.Write(chip != "" ? $" {chip}  |" : "    |");
                }
            }
            Console.WriteLine("\n   +---+----+----+----+----+----+----+");
        }

        // player makes a move 
        public bool MakeMove(Player player)
        {
            Console.WriteLine(player.PlayersName + ", it's your turn to play . Please choose a column from the letters  A to G: ");
            string input = Console.ReadLine();
            int[] coordinates = CoordinateParser(input);

            if (coordinates == null || !IsSpaceAvailable(coordinates))
            {
                Console.WriteLine(" This is an invalid move. Please try again.");
                return false;
            }

            ModifyArray(coordinates, player.GameChip);
            PrintGameBoard();

            return CheckForWinner(player);
        }

        // checks for a winner 
        public bool CheckForWinner(Player player)
        {
            string chip = player.GameChip;

            // checks rows
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols - 3; col++)
                {
                    if (gameBoard[row, col] == chip &&
                        gameBoard[row, col + 1] == chip &&
                        gameBoard[row, col + 2] == chip &&
                        gameBoard[row, col + 3] == chip)
                    {
                        return true;
                    }
                }
            }

            // checks the  columns
            for (int col = 0; col < Cols; col++)
            {
                for (int row = 0; row < Rows - 3; row++)
                {
                    if (gameBoard[row, col] == chip &&
                        gameBoard[row + 1, col] == chip &&
                        gameBoard[row + 2, col] == chip &&
                        gameBoard[row + 3, col] == chip)
                    {
                        return true;
                    }
                }
            }

            // cshecks diagonal  pattern 1 from the top left of the board  to bottom right.
            for (int row = 0; row < Rows - 3; row++)
            {
                for (int col = 0; col < Cols - 3; col++)
                {
                    if (gameBoard[row, col] == chip &&
                        gameBoard[row + 1, col + 1] == chip &&
                        gameBoard[row + 2, col + 2] == chip &&
                        gameBoard[row + 3, col + 3] == chip)
                    {
                        return true;
                    }
                }
            }

            // checks diagonal pattern 2  from the top right  of the board to the  bottom left.
            for (int row = 0; row < Rows - 3; row++)
            {
                for (int col = Cols - 1; col >= 3; col--)
                {
                    if (gameBoard[row, col] == chip &&
                        gameBoard[row + 1, col - 1] == chip &&
                        gameBoard[row + 2, col - 2] == chip &&
                        gameBoard[row + 3, col - 3] == chip)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // this checks  if  there's a space  available in the game board 
        public bool IsSpaceAvailable(int[] expectedCoordinate)
        {
            int column = expectedCoordinate[0];
            int row = expectedCoordinate[1];

            if (column < 0 || column >= Cols || row < 0 || row >= Rows)
            {
                return false;
            }

            return gameBoard[row, column] == "";
        }

        //this checks  if gravity lets player  place  a chip in the expected  column in the game board 
        public bool GravityChecker(int[] expectedCoordinate)
        {
            int column = expectedCoordinate[0];
            int row = expectedCoordinate[1];

            if (column < 0 || column >= Cols || row < 0 || row >= Rows)
            {
                return false;
            }

            return gameBoard[row, column] == "";
        }

        //  the game board's array with the player's chip
        public void ModifyArray(int[] expectedCoordinate, string chip)
        {
            int column = expectedCoordinate[0];
            int row = expectedCoordinate[1];

            gameBoard[row, column] = chip;
        }

        // Parses the user's input for the both column and row (coordinates)
        public int[] CoordinateParser(string input)
        {
            if (input.Length != 2)
                return null;

            int column = Array.IndexOf(PossibleLetters, input[0].ToString().ToUpper());
            int row = input[1] - '0';

            if (column == -1 || row < 0 || row >= Rows)
                return null;

            return new int[] { column, row };
        }
    }


    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Connect Four Game!");

            ConnectFourGame game = new ConnectFourGame();

            // this gives t the players name
            Console.Write("Enter the name of the first player : ");
            string player1Name = Console.ReadLine();
            Player player1 = new HumanPlayer(player1Name, "X");

            Console.Write("Enter the name of the second player: ");
            string player2Name = Console.ReadLine();
            Player player2 = new HumanPlayer2(player2Name, "O");

            Player currentPlayer = player1;
            bool gameEnded = false;

            // while loop
            while (!gameEnded)
            {
                game.PrintGameBoard();
                gameEnded = game.MakeMove(currentPlayer);

                if (gameEnded)
                {
                    Console.WriteLine(currentPlayer.PlayersName + " is  the winner of the game!");
                    break;
                }

                currentPlayer = (currentPlayer == player1) ? player2 : player1;
            }

            Console.WriteLine("\n This round of the  Game is  Over!");
            Console.ReadLine();
        }
    }
}
