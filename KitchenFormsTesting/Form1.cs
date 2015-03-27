using System;
using System.Windows.Forms;
using Kitchen.Shortcuts;

namespace KitchenFormsTesting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var shortcut = new ShellShortcut(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Wolfram Mathematica\Wolfram Mathematica 10.lnk");
            iconPictureBox.Image = shortcut.Icon.ToBitmap();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
