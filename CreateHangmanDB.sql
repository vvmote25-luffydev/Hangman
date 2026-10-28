IF DB_ID('HangmanDB') IS NULL
BEGIN
CREATE DATABASE HangmanDB;
END;
GO

USE HangmanDB;
GO

IF OBJECT_ID('dbo.Words', 'U') IS NULL
BEGIN
CREATE TABLE dbo.Words
(
WordId INT IDENTITY(1,1) PRIMARY KEY,
Word VARCHAR(50) NOT NULL,
Difficulty VARCHAR(10) NOT NULL
);
END;
GO

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'APPLE', 'Easy'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'APPLE');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'MANGO', 'Easy'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'MANGO');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'TIGER', 'Easy'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'TIGER');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'HOUSE', 'Easy'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'HOUSE');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'GRAPE', 'Easy'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'GRAPE');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'PYTHON', 'Medium'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'PYTHON');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'GUITAR', 'Medium'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'GUITAR');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'PLANET', 'Medium'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'PLANET');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'CASTLE', 'Medium'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'CASTLE');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'ROCKET', 'Medium'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'ROCKET');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'ELEPHANT', 'Hard'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'ELEPHANT');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'NOTEBOOK', 'Hard'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'NOTEBOOK');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'ADVENTURE', 'Hard'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'ADVENTURE');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'CHOCOLATE', 'Hard'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'CHOCOLATE');

INSERT INTO dbo.Words (Word, Difficulty)
SELECT 'BUTTERFLY', 'Hard'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Words WHERE Word = 'BUTTERFLY');

SELECT WordId, Word, Difficulty
FROM dbo.Words
ORDER BY Difficulty, Word;
