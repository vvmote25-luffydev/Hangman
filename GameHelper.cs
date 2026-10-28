using System;

public static class GameHelper
{
    public static int GetDifficultyChoice()
    {
        while (true)
        {
            Console.Write("Enter your choice (1-3): ");

            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out int choice))
            {
                if (choice >= 1 && choice <= 3)
                {
                    return choice;
                }
            }

            Console.WriteLine("Invalid choice! Please enter 1, 2, or 3.");
        }
    }

    public static char GetLetterInput()
    {
        while (true)
        {
            Console.Write("Guess a letter: ");

            string input = Console.ReadLine() ?? "";
            input = input.Trim();

            if (input.Length == 1 && char.IsLetter(input[0]))
            {
                return char.ToUpperInvariant(input[0]);
            }

            Console.WriteLine("Invalid input! Enter one letter.");
        }
    }
}