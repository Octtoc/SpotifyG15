using GDIMusic.Controls;
using LogitechLcdWrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GDIMusic
{
    public class GDIDynamic
    {
        private System.Timers.Timer _timer = new System.Timers.Timer();
        private List<IGDIControl> gdiControlList = new List<IGDIControl>();

        public GDIDynamic(Bitmap xBmpGraphics, bool logitechLcd = true)
        {
            LogitechLcd = logitechLcd;
            Bitmap = xBmpGraphics;
            Graphics = Graphics.FromImage(xBmpGraphics);
            Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            CurrentFont = new Font("Courier", 7);
            CurrentBrush = Brushes.Black;
            CurrentPen = new Pen(Color.Black);

            if (LogitechLcd)
            {
                LogitechGSDK.LogiLcdInit("G15 TestApp", LogitechGSDK.LOGI_LCD_TYPE_MONO);
            }

            _timer.Elapsed += new ElapsedEventHandler(ATimer_Elapsed);

            _timer.Interval = 200;
            _timer.Enabled = true;
            _timer.Start();
        }

        ~GDIDynamic()
        {
            CloseLcd();
        }

        public Bitmap Bitmap { get; }
        public Brush CurrentBrush { get; }
        public Font CurrentFont { get; }
        public Pen CurrentPen { get; }
        public Graphics Graphics { get; }
        public bool LogitechLcd { get; }

        public void AddControl(IGDIControl gdiControl)
        {
            gdiControlList.Add(gdiControl);
        }

        public void CloseLcd()
        {
            if (LogitechLcd)
            {
                LogitechGSDK.LogiLcdShutdown();
            }
        }

        private void ATimer_Elapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Graphics.Clear(Color.White);

            /*Test Drawings
            Graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(10, 30, 140, 5));
            Graphics.FillRectangle(CurrentBrush, new Rectangle(10, 30, 120, 5));
            Graphics.DrawEllipse(CurrentPen, new Rectangle(10, 10, 20, 20));
            */

            foreach (var gdiControl in gdiControlList)
            {
                gdiControl.Draw(Graphics, CurrentBrush, CurrentFont, CurrentPen);
            }

            if (LogitechLcd)
            {
                FillFromBitmap();
            }

            //Bitmap.Save(@"C:\temp\testg.bmp");
        }

        private void FillFromBitmap()
        {
            int bufferSize = (LogitechGSDK.LOGI_LCD_MONO_WIDTH * LogitechGSDK.LOGI_LCD_MONO_HEIGHT);
            byte[] pixelMatrix = new byte[bufferSize];

            for (int y = 0; y < LogitechGSDK.LOGI_LCD_MONO_HEIGHT; ++y)
            {
                for (int x = 0; x < LogitechGSDK.LOGI_LCD_MONO_WIDTH; ++x)
                {
                    Color trueColor = Bitmap.GetPixel(x, y);
                    byte nColor = (byte)((trueColor.R == 255 && trueColor.G == 255 && trueColor.B == 255) ? 0 : 255);

                    pixelMatrix[x + (y * 160)] = nColor;
                }
            }
            LogitechGSDK.LogiLcdMonoSetBackground(pixelMatrix);
            LogitechGSDK.LogiLcdUpdate();
        }
    }
}