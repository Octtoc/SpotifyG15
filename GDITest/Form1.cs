using GDIMusic.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDITest
{
    public partial class Form1 : Form
    {
        private static GDIMusic.GDIDynamic gdiDynamic;

        public Form1()
        {
            InitializeComponent();

            gdiDynamic = new GDIMusic.GDIDynamic(new Bitmap(160, 43), false);
            gdiDynamic.AddControl(new GDIText(new Rectangle(10, 10, 10, 10), "Text"));
            pictureBox1.Image = gdiDynamic.Bitmap;

            timer1.Enabled = true;
            timer1.Interval = 100;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
    }
}