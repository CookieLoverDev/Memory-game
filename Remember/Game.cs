using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Remember
{
    /// <summary>
    /// This part of the code is from the main game window
    /// there are only three blocks of code, not counting the IntializeComponent
    /// button1 is responsible for starting the game or if the conditions are met, then it restarts the game with the same parametrs
    /// button2 is responsible for going back to game menu
    /// Gmae_Load method just defines the game labels and buttons text
    /// </summary>
    public partial class Game : Form
    {
        public Game()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 menu = new Form1();
            menu.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GameLogic.preGame == true || GameLogic.inGame == true)
            {
                GameLogic.StartTimer();
            }
            else
            {
                GameLogic.Restart(panel2);
            }
        }

        private void Game_Load(object sender, EventArgs e)
        {
            label6.Text = GameLogic.playerName;
            GameLogic.GameSetUp(label8, label7, panel2);
            GameLogic.TimerSetUp(label2, button1);
            GameLogic.recieveLivesLabel(label8);
            GameLogic.recieveTilesLabel(label7);
        }
    }
}
