﻿using System;
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
    public partial class Form2 : Form
    {
        private Form1 Main = null;
        public Form2(Form Man)
        {
            Main = Man as Form1;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main.Difficulty = (int)numericUpDown1.Value;
            Main.Invalidate();
            Main.Paintrun = true;
            this.Close();
        }
    }
}
