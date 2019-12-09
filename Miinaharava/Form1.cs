using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miinaharawa
{
    public partial class Form1 : Form
    {
        public int Difficulty = 0;
        public bool Paintrun = false;
        private int Mines = 0;
        private List<string> MineLocation = new List<string>();
        private List<Control> AlreadyCleared = new List<Control>();
        private List<Control> Flagged = new List<Control>();
        Random rnd = new Random();

        public Form1()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Loads the difficulty selector form
            Form2 form2 = new Form2(this)
            {
                TopMost = true
            };
            form2.Show();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (Paintrun)
            {
                // Sets things up depending on the difficulty selected
                switch (Difficulty)
                {
                    case 1:
                        tableLayoutPanel1.ColumnCount = 9;
                        tableLayoutPanel1.RowCount = 9;
                        Mines = 10;
                        break;
                    case 2:
                        tableLayoutPanel1.ColumnCount = 16;
                        tableLayoutPanel1.RowCount = 16;
                        this.Width += 170;
                        this.Height += 170;
                        Mines = 40;
                        break;
                    case 3:
                        tableLayoutPanel1.ColumnCount = 30;
                        tableLayoutPanel1.RowCount = 16;
                        this.Width += 535;
                        this.Height += 170;
                        Mines = 99;
                        break;
                }

                FlagNum.Text = Convert.ToString(Mines);

                // Creates the buttons
                for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
                {
                    for (int j = 0; j < tableLayoutPanel1.RowCount; j++)
                    {
                        var Tile = new Square();
                        Tile.Name = string.Format("Tile_{0}-{1}", i, j);
                        Tile.Size = new Size(20, 20);
                        Tile.MouseUp += new System.Windows.Forms.MouseEventHandler(TileClick);
                        Tile.BackColor = Color.Gray;
                        tableLayoutPanel1.Controls.Add(Tile);
                    }
                }
                Game();
            }
        }

        private List<Control> SurroundCheck(string Tilename, int A, int B)
        {
            // Checks all of the surrounding tiles


            List<Control> TileList = new List<Control>();

            // Runs 2 times, on -1 Y and +1 Y, sets up the Y cordinates for the X loop
            for (int j = -1; j < 3; j++)
            {
                int Y = B + j;

                // Checks if the Y cordinate is still in the playfield
                if (Y >= 0 && Y < tableLayoutPanel1.RowCount)
                {
                    // The X loop. Runs 3 times from -1 to 1, creates one side of the mine
                    for (int l = -1; l < 2; l++)
                    {
                        int X = A + l;

                        // Checks that the X cordinate is still in the playfield
                        if (X >= 0 && X < tableLayoutPanel1.ColumnCount)
                        {
                            Tilename = string.Format("Tile_{0}-{1}", X, Y);

                            var chosentile = tableLayoutPanel1.Controls.Find(Tilename, false);

                            TileList.Add(chosentile[0]);
                        }
                    }
                }

                // Creates the top and bottom numbers, runs on the Y loop so it repeats twice, 
                // uses the Y loops number for calulations.
                if (A + j >= 0 && A + j < tableLayoutPanel1.ColumnCount)
                {
                    Tilename = string.Format("Tile_{0}-{1}", A + j, B);

                    var chosentile = tableLayoutPanel1.Controls.Find(Tilename, false);

                    TileList.Add(chosentile[0]);
                }
                j++;
            }
            return TileList;
        }

        private void Game()
        {
            Paintrun = false;

            for (int i = 0; i < Mines; i++)
            {
                // Gets a random location to place a mine in
                int A = rnd.Next(tableLayoutPanel1.ColumnCount);
                int B = rnd.Next(tableLayoutPanel1.RowCount);

                string tilename = string.Format("Tile_{0}-{1}", A, B);

                // Checks if the random location already has a mine on it
                var match = MineLocation.FirstOrDefault(MineLocation => MineLocation.Contains(tilename));

                if (match == null)
                {
                    var Tile = tableLayoutPanel1.Controls.Find(tilename, true);

                    // Little cheat by using the AccessibleName as a secondary tag to avoid messing with the number system
                    Tile[0].AccessibleName = "M";
                    Tile[0].BackgroundImage = Image.FromFile("C:\\Users\\Findu\\source\\repos\\valikkotehtava\\Miinaharawa\\BaldBoomer.png"); //debug


                    MineLocation.Add(tilename);

                    var chosentile = SurroundCheck(tilename, A, B);

                    foreach (var T in chosentile)
                    {
                        if (T.AccessibleName != "M")
                        {
                            if (T.Tag == null)
                            {
                                T.Tag = 1;
                            }
                            else
                            {
                                T.Tag = Convert.ToInt32(T.Tag) + 1;
                            }
                            T.Text = Convert.ToString(T.Tag); // debug
                        }
                    }
                }
                else
                {
                    i--;
                }
            }


        }

        public void TileClick(object sender, MouseEventArgs M)
        {
            Square B = sender as Square;
            

            // System.Windows.Forms.MessageBox.Show(B.Name + " ," + B.Tag + " ," + B.Text); // debug

            if (M.Button == MouseButtons.Right)
            {
                if (!Flagged.Contains(B) && FlagNum.Text != "0")
                {
                    B.BackgroundImage = Image.FromFile(@"C:\Users\Findu\Downloads\Flag.png");
                    Flagged.Add(B);
                    FlagNum.Text = Convert.ToString(Convert.ToInt32(FlagNum.Text) - 1);
                }
                else if (Flagged.Contains(B))
                {
                    B.BackgroundImage = null;
                    Flagged.Remove(B);
                    FlagNum.Text = Convert.ToString(Convert.ToInt32(FlagNum.Text) + 1);
                }

               
            }

            if (M.Button == MouseButtons.Left)
            {
                if (B.AccessibleName == "M")
                {
                    DialogResult result = System.Windows.Forms.MessageBox.Show("hävisit pelin");
                    
                    if (result == DialogResult.OK)
                    {
                        button1_Click(sender, M);
                    }
                }
                else if (int.TryParse(Convert.ToString(B.Tag), out int n))
                {
                    B.BackColor = Color.White;
                    B.Text = Convert.ToString(n);
                    if (!AlreadyCleared.Contains(B)) { AlreadyCleared.Add(B); }
                }
                else
                {
                    //actually empties most of the board on click

                    B.BackColor = Color.White;
                    AlreadyCleared.Add(B);

                    var X = tableLayoutPanel1.GetPositionFromControl(B).Row;
                    var Y = tableLayoutPanel1.GetPositionFromControl(B).Column;

                    if (!AlreadyCleared.Contains(B)) { AlreadyCleared.Add(B); }

                    var chosentiles = SurroundCheck(B.Name, X, Y);
                    foreach (var T in chosentiles)
                    {
                        if (!AlreadyCleared.Any(item => item.Name == T.Name) && T.AccessibleName != "M")
                        {
                            T.BackColor = Color.White;
                            AlreadyCleared.Add(T);
                            TileClick(T, M);
                        }
                    }
                }
            }

            if (Mines == Flagged.Count)
            {
                if (Flagged.All(Item => Item.AccessibleName == "M"))
                {
                    DialogResult result = System.Windows.Forms.MessageBox.Show("Voitit pelin!");

                    if (result == DialogResult.OK)
                    {
                        button1_Click(sender, M);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Restarts the game
            Paintrun = false;
            tableLayoutPanel1.Controls.Clear();
            Difficulty = 0;
            Mines = 0;
            MineLocation.Clear();
            this.Invalidate();
            Form1_Load(sender, e);
        }
    }
}
