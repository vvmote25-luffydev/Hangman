using System;

class Program
{
static void Main(string[] args)
{
Console.Title = "Hangman Word Guessing Game";

    bool playAgain = true;

    while (playAgain)
    {
        Console.Clear();

        Console.WriteLine("================================");
        Console.WriteLine("     HANGMAN WORD GUESS GAME");
        Console.WriteLine("================================");
        Console.WriteLine();

        Console.WriteLine("Choose Difficulty:");
        Console.WriteLine("1. Easy   (8 attempts)");
        Console.WriteLine("2. Medium (6 attempts)");
        Console.WriteLine("3. Hard   (4 attempts)");
        Console.WriteLine();

        int choice = GameHelper.GetDifficultyChoice();

        GameSettings settings = GameSettings.Create(choice);

        HangmanGame game = new HangmanGame(settings);

        game.Play();

        Console.WriteLine();
        Console.Write("Do you want to play again? (Y/N): ");

        string answer = Console.ReadLine() ?? "";

        playAgain = answer.Trim().Equals(
            "Y",
            StringComparison.OrdinalIgnoreCase
        );
    }

    Console.WriteLine();
    Console.WriteLine("Thank you for playing Hangman!");
    Console.WriteLine("Goodbye!");
}


}
