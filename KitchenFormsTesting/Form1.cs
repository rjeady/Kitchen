using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kitchen.ShellLink;

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
