using System;
using System.Collections.Generic;
using System.Text;

public class HangmanGame
{
private readonly GameSettings settings;
private string secretWord = "";
private readonly HashSet<char> guessedLetters = new HashSet<char>();
private int remainingAttempts;

public HangmanGame(GameSettings settings)
{
    this.settings = settings;
}

public void Play()
{
    try
    {
        secretWord = WordDatabase.GetRandomWord(settings.Difficulty);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Could not load a word from the database.");
        Console.WriteLine("Error: " + ex.Message);
        return;
    }

    guessedLetters.Clear();
    remainingAttempts = settings.MaxAttempts;

    Console.Clear();

    Console.WriteLine("================================");
    Console.WriteLine("         HANGMAN GAME");
    Console.WriteLine("================================");
    Console.WriteLine("Difficulty: " + settings.LevelName);
    Console.WriteLine("Maximum Attempts: " + remainingAttempts);
    Console.WriteLine();

    DisplayHangman();

    while (remainingAttempts > 0 && !IsWordGuessed())
    {
        DisplayWord();

        Console.WriteLine("Remaining attempts: " + remainingAttempts);
        Console.WriteLine("Guessed letters: " + GetGuessedLetters());

        char letter = GameHelper.GetLetterInput();

        if (guessedLetters.Contains(letter))
        {
            Console.WriteLine("You already guessed that letter!");
            Console.WriteLine();
            continue;
        }

        guessedLetters.Add(letter);

        if (secretWord.Contains(letter))
        {
            Console.WriteLine("Correct guess!");
        }
        else
        {
            remainingAttempts--;
            Console.WriteLine("Wrong guess!");
        }

        Console.WriteLine();
        DisplayHangman();
    }

    Console.WriteLine();
    DisplayWord();

    if (IsWordGuessed())
    {
        Console.WriteLine();
        Console.WriteLine("Congratulations! You won!");
        Console.WriteLine("You guessed the word correctly!");
    }
    else
    {
        Console.WriteLine();
        Console.WriteLine("Game Over!");
        Console.WriteLine("The correct word was: " + secretWord);
    }
}

private void DisplayWord()
{
    StringBuilder display = new StringBuilder();

    foreach (char letter in secretWord)
    {
        if (guessedLetters.Contains(letter))
        {
            display.Append(letter);
        }
        else
        {
            display.Append('_');
        }

        display.Append(' ');
    }

    Console.WriteLine("Word: " + display);
}

private bool IsWordGuessed()
{
    foreach (char letter in secretWord)
    {
        if (!guessedLetters.Contains(letter))
        {
            return false;
        }
    }

    return true;
}

private string GetGuessedLetters()
{
    List<char> letters = new List<char>(guessedLetters);

    letters.Sort();

    return string.Join(", ", letters);
}

private void DisplayHangman()
{
    int wrongGuesses = settings.MaxAttempts - remainingAttempts;

    string[] stages =
    {
        " +---+\n |   |\n     |\n     |\n     |\n_____|\n",
        " +---+\n |   |\n O   |\n     |\n     |\n_____|\n",
        " +---+\n |   |\n O   |\n |   |\n     |\n_____|\n",
        " +---+\n |   |\n O   |\n/|   |\n     |\n_____|\n",
        " +---+\n |   |\n O   |\n/|\\  |\n     |\n_____|\n",
        " +---+\n |   |\n O   |\n/|\\  |\n/    |\n_____|\n",
        " +---+\n |   |\n O   |\n/|\\  |\n/ \\  |\n_____|\n",
        " +---+\n |   |\n X   |\n/|\\  |\n/ \\  |\n_____|\n"
    };

    int stage = Math.Min(wrongGuesses, stages.Length - 1);

    Console.WriteLine(stages[stage]);
}

}
