public class GameSettings
{
public int Difficulty { get; private set; }

public string LevelName { get; private set; }

public int MaxAttempts { get; private set; }

private GameSettings(int difficulty, string levelName, int maxAttempts)
{
    Difficulty = difficulty;
    LevelName = levelName;
    MaxAttempts = maxAttempts;
}

public static GameSettings Create(int difficulty)
{
    switch (difficulty)
    {
        case 1:
            return new GameSettings(1, "Easy", 8);

        case 2:
            return new GameSettings(2, "Medium", 6);

        case 3:
            return new GameSettings(3, "Hard", 4);

        default:
            throw new System.ArgumentOutOfRangeException(
                nameof(difficulty),
                "Invalid difficulty level."
            );
    }
}
}
