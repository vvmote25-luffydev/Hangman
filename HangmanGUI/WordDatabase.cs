using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace HangmanGUI
{
    public class WordDatabase
    {
        private readonly string connectionString =
            @"Server=.\SQLEXPRESS;Database=HangmanDB;Integrated Security=True;TrustServerCertificate=True;";

        public List<(string Word, string Difficulty)> GetWords(string difficulty)
        {
            List<(string Word, string Difficulty)> words = new List<(string, string)>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT Word, Difficulty
                                 FROM dbo.Words
                                 WHERE Difficulty = @Difficulty";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Difficulty", difficulty);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string word = reader["Word"].ToString() ?? "";
                            string level = reader["Difficulty"].ToString() ?? "";

                            words.Add((word, level));
                        }
                    }
                }
            }

            return words;
        }
    }
}