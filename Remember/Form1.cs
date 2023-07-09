using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace Remember
{
    /// <summary>
    /// This block of code is the menu window
    /// This code is responsible to taking game parametrs, such as board size, show time, number of tiles platyer needs to find and the lives player has
    /// There are also pre-defined game parametrs, which are labeled by difficulity as "Easy", "Normal" and "Hard"
    /// This code also passes all the parametrs to the GameLogic.cs Class for the the game creation and possible restart
    /// Name and the difficulity are also passed in order to save the results
    /// </summary>
    public partial class Form1 : Form
    {
        int size;
        public Form1()
        {
                InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            GameLogic.gameMode = comboBox1.SelectedItem.ToString();
            if (textBox10.Text == string.Empty)
            {
                MessageBox.Show("Please Enter a player name!", "No Player Name!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                GameLogic.playerName = textBox10.Text;
                try
                {
                    size = Convert.ToInt32(textBox4.Text);
                    if (size <= 11)
                    {
                        GameLogic.boardSize = Convert.ToInt32(textBox4.Text);
                        GameLogic.playerLives = Convert.ToInt32(textBox3.Text);
                        GameLogic.timerTime = Convert.ToInt32(textBox2.Text);
                        GameLogic.tilesLeft = Convert.ToInt32(textBox1.Text);
                        this.Hide();
                        Game game = new Game();
                        game.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Max board size should not exceed 11","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch 
                {
                    MessageBox.Show("Please write only numbers and fill all settings", "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox[] textBoxes = { textBox4, textBox3, textBox2, textBox1 };

            if (comboBox1.SelectedIndex == 0)
            {
                textBox4.Text = "5";
                textBox3.Text = "3";
                textBox2.Text = "5";
                textBox1.Text = "5";
            }

            else if (comboBox1.SelectedIndex == 1)
            {

                textBox4.Text = "7";
                textBox3.Text = "2";
                textBox2.Text = "3";
                textBox1.Text = "7";
            }

            else if (comboBox1.SelectedIndex == 2)
            {
                textBox4.Text = "9";
                textBox3.Text = "2";
                textBox2.Text = "3";
                textBox1.Text = "11";
            }

            else
            {
                foreach (var textBox in textBoxes)
                {
                    textBox.ReadOnly = false;
                }
                textBox4.Text = "";
                textBox3.Text = "";
                textBox2.Text = "";
                textBox1.Text = "";
            }
        }

    }
}
