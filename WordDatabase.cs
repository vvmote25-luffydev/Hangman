using System;
using Microsoft.Data.SqlClient;

public static class WordDatabase
{
private static readonly string connectionString =
@"Server=.\SQLEXPRESS;Database=HangmanDB;Integrated Security=True;TrustServerCertificate=True;";


public static string GetRandomWord(int difficulty)
{
    string difficultyName;

    switch (difficulty)
    {
        case 1:
            difficultyName = "Easy";
            break;

        case 2:
            difficultyName = "Medium";
            break;

        case 3:
            difficultyName = "Hard";
            break;

        default:
            throw new ArgumentOutOfRangeException(nameof(difficulty));
    }

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        string query =
            "SELECT TOP (1) Word FROM dbo.Words " +
            "WHERE Difficulty = @Difficulty ORDER BY NEWID();";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Difficulty", difficultyName);

            object? result = command.ExecuteScalar();

            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException(
                    "No words found for difficulty: " + difficultyName);
            }

            return result.ToString()!.ToUpperInvariant();
        }
    }
}

}
