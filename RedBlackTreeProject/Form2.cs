﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RedBlackTreeProject
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            checkBox.Checked = !Properties.Settings.Default.isShowAboutMenu;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.isShowAboutMenu = !checkBox.Checked;
            Properties.Settings.Default.Save();

            this.Close();
        }
    }
}
