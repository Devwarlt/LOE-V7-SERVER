using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Game_Launcher
{
    public partial class Launcher : Form
    {
        public Launcher()
        {
            InitializeComponent();
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            Process.Start("");
        }
    }
}
