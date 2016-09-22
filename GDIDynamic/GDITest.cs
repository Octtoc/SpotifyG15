using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GDIMusic
{
    public partial class GDITest : Form
    {
        private static GDIDynamic gdiDynamic;
        public GDITest(GDIDynamic xGdiDynamic)
        {
            InitializeComponent();
            gdiDynamic = xGdiDynamic;

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
