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

            var shortcut = new ShellShortcut(@"C:\Users\Robert\Desktop\Audacity.lnk");
            iconPictureBox.Image = shortcut.Icon.ToBitmap();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
