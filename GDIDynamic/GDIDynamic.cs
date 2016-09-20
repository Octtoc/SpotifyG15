using GDIDynamic.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GDIDynamic
{
    public class GDIDynamic
    {
        private System.Timers.Timer _timer = new System.Timers.Timer();
        private List<IGDIControl> gdiControlList = new List<IGDIControl>();

        public GDIDynamic(Bitmap xBmpGraphics)
        {
            Bitmap = xBmpGraphics;
            Graphics = Graphics.FromImage(xBmpGraphics);
            Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            CurrentFont = new Font("Courier", 7);
            CurrentBrush = Brushes.Black;
            CurrentPen = new Pen(Color.Black);

            _timer.Elapsed += new ElapsedEventHandler(ATimer_Elapsed);

            _timer.Interval = 100;
            _timer.Enabled = true;
            _timer.Start();
        }

        public Bitmap Bitmap { get; }
        public Brush CurrentBrush { get; }
        public Font CurrentFont { get; }
        public Pen CurrentPen { get; }
        public Graphics Graphics { get; }

        public void AddControl(IGDIControl gdiControl)
        {
            gdiControlList.Add(gdiControl);
        }

        private void ATimer_Elapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Graphics.Clear(Color.Transparent);

            /*Test Drawings
            Graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(10, 30, 140, 5));
            Graphics.FillRectangle(CurrentBrush, new Rectangle(10, 30, 120, 5));
            Graphics.DrawEllipse(CurrentPen, new Rectangle(10, 10, 20, 20));
            */

            foreach (var gdiControl in gdiControlList)
            {
                gdiControl.Draw(Graphics, CurrentBrush, CurrentFont, CurrentPen);
            }

            //Bitmap.Save(@"C:\temp\testg.bmp");
        }
    }
}