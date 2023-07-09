using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remember
{
    /// <summary>
    /// This class is responsible for all the game logic
    /// This class dynamically creates the panel on the game window and creates and puts the game buttons into the panel the same way
    /// The game buttons are stored in the list and when the game starts class gives the certain number of random buttons values from 1 to the number of tiles
    /// number of tiles are set by player in the game menu
    /// During the preGame, button are noninteractive and the correct order is set to visible
    /// During the inGame numbers set again to non-visible and buttons are interactive
    /// Player should press them in correct order
    /// When button is pressed in correct order, it changes its color to green and value becomes visible, otherwise color changes to red
    /// Game is over when all the buttons with values are pressed in correct order or when plater runs out of lives
    /// Results of the game can be saved after the game is over
    /// Results are saved in a txt file in the root of the application
    /// </summary>
    public static class GameLogic
    {
        public static string gameMode { get; set; }
        public static string playerName { get; set; }
        public static int playerLives { get; set; }
        public static int timerTime { get; set; }
        public static int tilesLeft { get; set; }
        public static int boardSize { get; set; }

        private static int playerLivesRestart;
        private static int tilesLeftRestart;
        private static int timerTimeRestart;

        private static List<Button> gameButtons = new List<Button>();
        private static string[] buttonValues;

        public static bool preGame = true;
        public static bool inGame = false;

        private static Timer gameTimer;
        private static Label updatedTime;
        private static Button gameStartButton;
        private static Label updatedLives;
        private static Label updatedTiles;

        private static Random random = new Random();
        private static int xButtons = 10;
        private static int yButtons = 10;

        private static int i = 1;
        private static Label lives;
        private static Label tiles;
        private static string result;

        private static Panel Mgame = new Panel();

        private static void ChangeLives(Label livesLabel)
        {
            updatedLives = livesLabel;
            updatedLives.Text = playerLives.ToString();
        }

        private static void ChangeTiles(Label tilesLabel)
        {
            updatedTiles = tilesLabel;
            updatedTiles.Text = tilesLeft.ToString();
        }

        private static void GameWindowCreation(Panel panel)
        {
            Mgame = new Panel();
            Mgame = panel;
            Mgame.Controls.Clear();
            CreateOrderValues();
            GameWindowButtons();
        }

        private static void CreateOrderValues()
        {
            buttonValues = new string[tilesLeft];
            for (int i = 0; i < tilesLeft; i++)
            {
                string value = (i + 1).ToString();
                buttonValues[i] = value;
            }
        }

        public static void GameWindowButtons()
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    Button btn = new Button();

                    btn.Name = $"btn_{i}_{j}";
                    btn.Text = "";
                    btn.Size = new Size(60, 60);
                    btn.BackColor = Color.Black;
                    btn.Font = new Font("Arial", 20, FontStyle.Bold);
                    btn.Enabled = false;
                    btn.Location = new Point(xButtons, yButtons);
                    btn.Click += (sender, e) => GamePlay(sender, e, btn);
                    gameButtons.Add(btn);
                    Mgame.Controls.Add(btn);
                    xButtons += 60;
                }
                xButtons = 10;
                yButtons += 60;
            }
            GameButtonValues();
            xButtons = 10;
            yButtons = 10;

        }

        private static void GameButtonValues()
        {
            int randomIndex;
            for (int i = 0; i < buttonValues.Length; i++)
            {
                randomIndex = random.Next(0, gameButtons.Count);
                if (gameButtons[randomIndex].Text == string.Empty)
                {
                    gameButtons[randomIndex].Text = buttonValues[i];
                }
                else
                    i--;
            }
        }

        public static void GameSetUp(Label livesLabel, Label tilesLabel, Panel panel)
        {
            ChangeLives(livesLabel);
            ChangeTiles(tilesLabel);
            GameWindowCreation(panel);
            RestartSetUp();
        }

        public static void TimerSetUp(Label timeLabel, Button button)
        {
            gameTimer = new Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += SecondTick;
            gameStartButton = button;
            updatedTime = timeLabel;
            UpdateTimerLabel();
        }

        public static void StartTimer()
        {
            gameTimer.Start();
            changeForeColor();
        }

        private static void StopTimer()
        {
            gameTimer.Stop();
        }

        private static void SecondTick(object sender, EventArgs e)
        {
            gameStartButton.Enabled = false;
            if (preGame == true && inGame == false)
            {
                timerTime--;
                UpdateTimerLabel();
                if (timerTime == 0)
                {
                    StopTimer();
                    preGame = false;
                    inGame = true;
                    changeForeColor();
                    StartTimer();
                }
            }
            else if (preGame == false && inGame == true)
            {
                if (tilesLeft <= 0 || playerLives <= 0)
                {
                    GameOver();
                }
                timerTime++;
                UpdateTimerLabel();
            }
            else
            {
                StopTimer();
                gameStartButton.Text = "Try Again";
                gameStartButton.ForeColor = Color.Black;
                gameStartButton.Enabled = true;
            }

        }

        private static void UpdateTimerLabel()
        {
            updatedTime.Text = timerTime.ToString();
        }

        private static void changeForeColor()
        {
            if (preGame == true && inGame == false)
            {
                for (int i = 0; i < gameButtons.Count; i++)
                {
                    gameButtons[i].Enabled = true;
                    gameButtons[i].ForeColor = Color.White;
                }
            }
            else if (preGame == false && inGame == true)
            {
                for (int i = 0; i < gameButtons.Count; i++)
                {
                    gameButtons[i].ForeColor = Color.Black;
                }
            }
        }

        public static void recieveLivesLabel(Label label)
        {
            lives = new Label();
            lives = label;
        }

        public static void recieveTilesLabel(Label label)
        {
            tiles = new Label();
            tiles = label;
        }

        private static void GamePlay(object sender, EventArgs e, Button btn)
        {
            if (preGame)
            {

            }
            else
            {
                if (btn.Text == i.ToString())
                {
                    tilesLeft--;
                    btn.BackColor = Color.Green;
                    btn.ForeColor = Color.Black;
                    ChangeTiles(tiles);
                    i++;
                }
                else
                {
                    playerLives--;
                    btn.BackColor = Color.Red;
                    btn.ForeColor = Color.Red;
                    ChangeLives(lives);
                }
            }
        }

        private static void GameOver()
        {
            DialogResult answer = new DialogResult();
            inGame = false;
            foreach (Button button in gameButtons)
            {
                if (button.Text != string.Empty)
                {
                    button.BackColor = Color.Green;
                }
                button.Enabled = false;
            }
            if (playerLives <= 0)
            {
                result = "";
                answer = MessageBox.Show("You lost, You can try again!\nDo you want to save the results?", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == DialogResult.Yes)
                {
                    GameWonSave();
                }
            }
            else
            {
                result = "won";
                answer = MessageBox.Show("You won, You can try again!\nDo you want to save the results?", "Game Over", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == DialogResult.Yes)
                {
                    GameWonSave();
                }
            }
        }

        public static void Restart(Panel panel)
        {
            playerLives = playerLivesRestart;
            timerTime = timerTimeRestart;
            tilesLeft = tilesLeftRestart;
            inGame = false;
            preGame = true;
            gameStartButton.Text = "Start the game";
            gameButtons.Clear();
            i = 1;
            UpdateTimerLabel();
            GameSetUp(lives, tiles,panel);
        }

        private static void RestartSetUp()
        {
            playerLivesRestart = playerLives;
            timerTimeRestart = timerTime;
            tilesLeftRestart = tilesLeft;
        }

        private static void GameWonSave()
        {
            FileStream fileStream = new FileStream("results.txt", FileMode.Append, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            if (result == "won")
            {
                streamWriter.WriteLine($"{playerName} has won the game! Difficulity: {gameMode}, Time: {timerTime}, Tiles: {tilesLeftRestart}");
            }
            else
            {
                streamWriter.WriteLine($"{playerName} has lost the game! Difficulity: {gameMode}, Time: {timerTime}, Tiles: {tilesLeftRestart}");
            }
            streamWriter.Close();
            fileStream.Close();
        }
    }
}
