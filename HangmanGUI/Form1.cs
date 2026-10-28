using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HangmanGUI
{
    public partial class Form1 : Form
    {
        private readonly Color background = Color.FromArgb(13, 15, 35);
        private readonly Color cardColor = Color.FromArgb(25, 28, 58);
        private readonly Color purple = Color.FromArgb(151, 91, 255);
        private readonly Color cyan = Color.FromArgb(76, 226, 255);
        private readonly Color white = Color.FromArgb(240, 243, 255);
        private readonly Color muted = Color.FromArgb(158, 169, 205);

        private Label wordLabel = null!;
        private Label mistakesLabel = null!;
        private Label messageLabel = null!;
        private Label hintLabel = null!;

        private FlowLayoutPanel keyboard = null!;
        private ComboBox difficulty = null!;
        private Panel drawingArea = null!;

        private Button hintButton = null!;
        private Button revealButton = null!;
        private Button playAgainButton = null!;

        private readonly Random random = new Random();
        private readonly PointF[] stars = new PointF[90];

        private readonly WordDatabase database = new WordDatabase();

        private string secretWord = "";
        private int mistakes;
        private int hintsUsed;

        private const int MaxMistakes = 6;
        private bool gameOver;

            private readonly Dictionary<string, string> wordHints =
            new Dictionary<string, string>
        {
            { "APPLE", "A fruit that can be red or green." },
            { "TIGER", "A large wild cat with black stripes." },
            { "MANGO", "A tropical fruit known as the king of fruits." },
            { "PIXEL", "A tiny unit that makes up a digital image." },
            { "ROBOT", "A machine that can perform tasks automatically." },
            { "SPACE", "The vast region beyond Earth's atmosphere." },
            { "MUSIC", "An art involving rhythm, melody, and sound." },
            { "PLANT", "A living thing that usually grows in soil." },

            { "GALAXY", "A huge collection of stars, gas, and dust." },
            { "PYTHON", "A programming language named after a comedy group." },
            { "PUZZLE", "A problem or game that tests your thinking." },
            { "PLANET", "A celestial body that orbits a star." },
            { "GAMING", "Playing video games as an activity." },
            { "JUNGLE", "A dense tropical forest." },
            { "CASTLE", "A fortified building where royalty once lived." },
            { "POCKET", "A small pouch in clothing." },

            { "ASTRONAUT", "A person trained to travel into space." },
            { "KEYBOARD", "A device used to type letters on a computer." },
            { "CHOCOLATE", "A sweet treat made from cacao." },
            { "ADVENTURE", "An exciting or unusual experience." },
            { "COMPUTER", "An electronic machine that processes data." },
            { "BUTTERFLY", "An insect with colourful wings." }
        };

        public Form1()
        {
            InitializeComponent();

            Text = "PIXELGAME | Word Guess";
            Size = new Size(1000, 800);
            MinimumSize = new Size(950, 780);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = background;
            ForeColor = white;
            Font = new Font("Segoe UI", 10);
            DoubleBuffered = true;

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new PointF(
                    random.Next(0, 1000),
                    random.Next(0, 800));
            }

            BuildInterface();
            StartNewGame();
        }

        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush starBrush =
                   new SolidBrush(Color.FromArgb(150, cyan)))
            {
                foreach (PointF star in stars)
                {
                    e.Graphics.FillEllipse(
                        starBrush,
                        star.X,
                        star.Y,
                        2,
                        2);
                }
            }

            using (Pen linePen =
                   new Pen(Color.FromArgb(45, purple), 1))
            {
                e.Graphics.DrawLine(
                    linePen,
                    30,
                    90,
                    ClientSize.Width - 30,
                    90);
            }
        }

            private void BuildInterface()
        {
            Controls.Clear();

            Label titleLabel = new Label
            {
                Text = "PIXELGAME",
                Font = new Font("Segoe UI", 25, FontStyle.Bold),
                ForeColor = cyan,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 15),
                Size = new Size(ClientSize.Width, 45),
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Left |
                         AnchorStyles.Right
            };

            Controls.Add(titleLabel);

            Label subtitle = new Label
            {
                Text = "GUESS THE WORD // ENTER THE UNKNOWN",
                Font = new Font("Consolas", 10),
                ForeColor = muted,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 60),
                Size = new Size(ClientSize.Width, 25),
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Left |
                         AnchorStyles.Right
            };

            Controls.Add(subtitle);

            Panel card = new Panel
            {
                BackColor = cardColor,
                Size = new Size(700, 490),
                Location = new Point(
                    (ClientSize.Width - 700) / 2,
                    115),
                Anchor = AnchorStyles.Top
            };

            Controls.Add(card);

            wordLabel = new Label
            {
                Text = "_ _ _ _ _",
                Font = new Font("Consolas", 28, FontStyle.Bold),
                ForeColor = cyan,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 20),
                Size = new Size(660, 65)
            };

            card.Controls.Add(wordLabel);

  
            mistakesLabel = new Label
            {
                Text = "LIVES REMAINING: 6",
                Font = new Font("Consolas", 12, FontStyle.Bold),
                ForeColor = purple,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 85),
                Size = new Size(660, 30)
            };

            card.Controls.Add(mistakesLabel);

            drawingArea = new Panel
            {
                BackColor = Color.FromArgb(18, 21, 45),
                Location = new Point(230, 120),
                Size = new Size(240, 135)
            };

            drawingArea.Paint += DrawingArea_Paint;
            card.Controls.Add(drawingArea);

            messageLabel = new Label
            {
                Text = "SELECT A LETTER TO BEGIN!",
                Font = new Font("Consolas", 10, FontStyle.Bold),
                ForeColor = cyan,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(15, 260),
                Size = new Size(670, 40)
            };

            card.Controls.Add(messageLabel);

            keyboard = new FlowLayoutPanel
            {
                Location = new Point(40, 305),
                Size = new Size(620, 115),
                BackColor = cardColor,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5)
            };

            card.Controls.Add(keyboard);

            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                Button button = new Button
                {
                    Text = letter.ToString(),
                    Tag = letter,
                    Size = new Size(39, 32),
                    Margin = new Padding(3),
                    BackColor = Color.FromArgb(42, 45, 82),
                    ForeColor = white,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Consolas", 10, FontStyle.Bold),
                    Cursor = Cursors.Hand
                };

                button.FlatAppearance.BorderColor = purple;
                button.FlatAppearance.BorderSize = 1;
                button.Click += LetterButton_Click;

                button.MouseEnter += (s, e) =>
                {
                    if (button.Enabled)
                    {
                        button.BackColor = purple;
                        button.ForeColor = Color.White;
                    }
                };

                button.MouseLeave += (s, e) =>
                {
                    if (button.Enabled)
                    {
                        button.BackColor = Color.FromArgb(42, 45, 82);
                        button.ForeColor = white;
                    }
                };

                keyboard.Controls.Add(button);
            }

            Label difficultyLabel = new Label
            {
                Text = "DIFFICULTY",
                Font = new Font("Consolas", 10, FontStyle.Bold),
                ForeColor = muted,
                Location = new Point(115, 625),
                Size = new Size(110, 32),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Controls.Add(difficultyLabel);

            difficulty = new ComboBox
            {
                Location = new Point(225, 625),
                Size = new Size(120, 32),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = cardColor,
                ForeColor = white,
                Font = new Font("Consolas", 10)
            };

            difficulty.Items.AddRange(
                new object[] { "Easy", "Medium", "Hard" });

            difficulty.SelectedIndex = 0;

            difficulty.SelectedIndexChanged += (s, e) =>
            {
                StartNewGame();
            };

            Controls.Add(difficulty);

        
            playAgainButton = new Button
            {
                Text = "PLAY AGAIN",
                Location = new Point(365, 622),
                Size = new Size(140, 38),
                BackColor = purple,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Consolas", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            playAgainButton.FlatAppearance.BorderSize = 0;
            playAgainButton.Click += (s, e) => StartNewGame();

            Controls.Add(playAgainButton);

            hintButton = new Button
            {
                Text = "HINT",
                Location = new Point(515, 622),
                Size = new Size(90, 38),
                BackColor = Color.FromArgb(25, 110, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Consolas", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            hintButton.FlatAppearance.BorderSize = 0;
            hintButton.Click += HintButton_Click;

            Controls.Add(hintButton);

            revealButton = new Button
            {
                Text = "REVEAL",
                Location = new Point(615, 622),
                Size = new Size(100, 38),
                BackColor = Color.FromArgb(90, 65, 145),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Consolas", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            revealButton.FlatAppearance.BorderSize = 0;
            revealButton.Click += RevealButton_Click;

            Controls.Add(revealButton);

            hintLabel = new Label
            {
                Text = "HINTS USED: 0",
                Font = new Font("Consolas", 9, FontStyle.Bold),
                ForeColor = cyan,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 675),
                Size = new Size(ClientSize.Width, 25),
                Anchor = AnchorStyles.Bottom |
                         AnchorStyles.Left |
                         AnchorStyles.Right
            };

            Controls.Add(hintLabel);

            Label footer = new Label
            {
                Text = "PIXELGAME // EVERY LETTER COUNTS",
                Font = new Font("Consolas", 9),
                ForeColor = muted,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 710),
                Size = new Size(ClientSize.Width, 25),
                Anchor = AnchorStyles.Bottom |
                         AnchorStyles.Left |
                         AnchorStyles.Right
            };

            Controls.Add(footer);
        }

        private void StartNewGame()
        {
            if (difficulty.SelectedItem == null)
            {
                return;
            }

            string selectedDifficulty =
                difficulty.SelectedItem.ToString() ?? "Easy";

            try
            {
                List<(string Word, string Difficulty)> words =
                    database.GetWords(selectedDifficulty);

                if (words.Count == 0)
                {
                    gameOver = true;

                    MessageBox.Show(
                        $"No words found for {selectedDifficulty} difficulty.",
                        "PIXELGAME",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                secretWord = words[random.Next(words.Count)]
                    .Word.Trim().ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(secretWord))
                {
                    MessageBox.Show(
                        "The selected database word is empty.",
                        "PIXELGAME",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }
            catch (Exception ex)
            {
                gameOver = true;

                MessageBox.Show(
                    "Could not load words from SQL Server.\n\n" +
                    "Check that SQL Server is running and the " +
                    "HangmanDB database exists.\n\n" +
                    "Details: " + ex.Message,
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            mistakes = 0;
            hintsUsed = 0;
            gameOver = false;

            foreach (Control control in keyboard.Controls)
            {
                if (control is Button button)
                {
                    button.Enabled = true;
                    button.BackColor = Color.FromArgb(42, 45, 82);
                    button.ForeColor = white;
                }
            }

            hintButton.Enabled = true;
            revealButton.Enabled = true;

            hintLabel.Text = "HINTS USED: 0";

            messageLabel.Text = "SELECT A LETTER TO BEGIN!";
            messageLabel.ForeColor = cyan;

            UpdateWordDisplay();
            UpdateMistakesDisplay();

            drawingArea.Invalidate();
        }

        private void LetterButton_Click(object? sender, EventArgs e)
        {
            if (gameOver || sender is not Button button)
            {
                return;
            }

            char guessedLetter = (char)button.Tag!;

            button.Enabled = false;

            if (secretWord.Contains(guessedLetter))
            {
                button.BackColor = Color.FromArgb(25, 110, 100);
                button.ForeColor = Color.White;

                messageLabel.Text = "NICE! THAT LETTER IS CORRECT.";
                messageLabel.ForeColor = cyan;
            }
            else
            {
                mistakes++;

                button.BackColor = Color.FromArgb(130, 40, 70);
                button.ForeColor = Color.White;

                messageLabel.Text = "OOPS! TRY ANOTHER LETTER.";
                messageLabel.ForeColor = Color.LightPink;
            }

            UpdateWordDisplay();
            UpdateMistakesDisplay();

            drawingArea.Invalidate();

            CheckGameStatus();
        }
        private void UpdateWordDisplay()
        {
            string display = "";

            foreach (char letter in secretWord)
            {
                bool guessed = false;

                foreach (Control control in keyboard.Controls)
                {
                    if (control is Button button &&
                        button.Tag is char selectedLetter &&
                        selectedLetter == letter &&
                        !button.Enabled)
                    {
                        guessed = true;
                        break;
                    }
                }

                display += guessed ? letter + " " : "_ ";
            }

            wordLabel.Text = display.TrimEnd();
        }

        private void CheckGameStatus()
        {
            if (IsWordGuessed())
            {
                gameOver = true;

                messageLabel.Text = "YOU WIN! WORD: " + secretWord;
                messageLabel.ForeColor = cyan;

                DisableKeyboard();
            }
            else if (mistakes >= MaxMistakes)
            {
                gameOver = true;

                wordLabel.Text = string.Join(
                    " ",
                    secretWord.ToCharArray());

                messageLabel.Text = "GAME OVER! WORD: " + secretWord;
                messageLabel.ForeColor = Color.LightPink;

                DisableKeyboard();
            }
        }

        private bool IsWordGuessed()
        {
            foreach (char letter in secretWord)
            {
                bool found = false;

                foreach (Control control in keyboard.Controls)
                {
                    if (control is Button button &&
                        button.Tag is char selectedLetter &&
                        selectedLetter == letter &&
                        !button.Enabled)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return secretWord.Length > 0;
        }

        // SHOW A WORD CLUE
        private void HintButton_Click(object? sender, EventArgs e)
        {
            if (gameOver)
            {
                return;
            }

            if (wordHints.TryGetValue(secretWord, out string? hint))
            {
                messageLabel.Text = "HINT: " + hint;
                messageLabel.ForeColor = cyan;

                hintsUsed++;
                hintLabel.Text = $"HINTS USED: {hintsUsed}";
            }
            else
            {
                messageLabel.Text =
                    "NO HINT AVAILABLE FOR THIS WORD YET.";

                messageLabel.ForeColor = muted;
            }
        }

        private void RevealButton_Click(object? sender, EventArgs e)
        {
            if (gameOver)
            {
                return;
            }

            foreach (char letter in secretWord)
            {
                foreach (Control control in keyboard.Controls)
                {
                    if (control is Button button &&
                        button.Tag is char selectedLetter &&
                        selectedLetter == letter &&
                        button.Enabled)
                    {
                      
                        button.Enabled = false;
                        button.BackColor = Color.FromArgb(25, 110, 100);
                        button.ForeColor = Color.White;
                        mistakes++;
                        hintsUsed++;

                        hintLabel.Text = $"HINTS USED: {hintsUsed}";

                        messageLabel.Text =
                            $"REVEALED: {letter} (1 LIFE USED)";

                        messageLabel.ForeColor = cyan;

                        UpdateWordDisplay();
                        UpdateMistakesDisplay();

                        drawingArea.Invalidate();

                        CheckGameStatus();

                        return;
                    }
                }
            }

            messageLabel.Text = "NO MORE LETTERS TO REVEAL!";
        }
        private void UpdateMistakesDisplay()
        {
            int remainingLives = Math.Max(0, MaxMistakes - mistakes);

            mistakesLabel.Text =
                $"LIVES REMAINING: {remainingLives}";
        }

        private void DisableKeyboard()
        {
            foreach (Control control in keyboard.Controls)
            {
                control.Enabled = false;
            }

            hintButton.Enabled = false;
            revealButton.Enabled = false;
        }
        private void DrawingArea_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen pen = new Pen(cyan, 3))
            {
      
                e.Graphics.DrawLine(pen, 35, 115, 115, 115);
                e.Graphics.DrawLine(pen, 55, 115, 55, 15);
                e.Graphics.DrawLine(pen, 55, 15, 145, 15);
                e.Graphics.DrawLine(pen, 145, 15, 145, 35);

              
                if (mistakes >= 1)
                {
                    e.Graphics.DrawEllipse(pen, 130, 35, 30, 30);
                }

               
                if (mistakes >= 2)
                {
                    e.Graphics.DrawLine(pen, 145, 65, 145, 90);
                }

                if (mistakes >= 3)
                {
                    e.Graphics.DrawLine(pen, 145, 72, 128, 82);
                }

                
                if (mistakes >= 4)
                {
                    e.Graphics.DrawLine(pen, 145, 72, 162, 82);
                }

                
                if (mistakes >= 5)
                {
                    e.Graphics.DrawLine(pen, 145, 90, 130, 108);
                }

                
                if (mistakes >= 6)
                {
                    e.Graphics.DrawLine(pen, 145, 90, 160, 108);
                }
            }
        }
    }
}