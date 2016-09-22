﻿using GDIMusic.Controls;
using LogitechLcdWrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace GDIMusic
{
    public class GDIDynamic
    {
        private readonly string G15APPNAME = "GMedia";
        private System.Timers.Timer _timer = new System.Timers.Timer();
        private List<IGDIControl> _gdiControlList = new List<IGDIControl>();

        public GDIDynamic(Bitmap xBmpGraphics, bool logitechLcd = true)
        {
            LogitechLcd = logitechLcd;
            Bitmap = xBmpGraphics;
            Graphics = Graphics.FromImage(xBmpGraphics);
            Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            CurrentFont = new Font("7px2bus", 7);
            CurrentBrush = Brushes.Black;
            CurrentPen = new Pen(Color.Black);

            /*
            //Create your private font collection object.
            PrivateFontCollection pfc = new PrivateFontCollection();
            //Select your font from the resources.
            //My font here is "Digireu.ttf"
            int fontLength = Properties.Resources._7pxbus.Length;
            // create a buffer to read in to
            byte[] fontdata = Properties.Resources._7pxbus;
            // create an unsafe memory block for the font data
            System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);
            // copy the bytes to the unsafe memory block
            Marshal.Copy(fontdata, 0, data, fontLength);
            // pass the font to the font collection
            pfc.AddMemoryFont(data, fontLength);
            // free up the unsafe memory
            Marshal.FreeCoTaskMem(data);
            */

            _timer.Elapsed += new ElapsedEventHandler(ATimer_Elapsed);

            _timer.Interval = 200;
            _timer.Enabled = true;
            _timer.Start();

            if (LogitechLcd)
            {
                LogitechGSDK.LogiLcdInit(G15APPNAME, LogitechGSDK.LOGI_LCD_TYPE_MONO);
            }
            else
            {
                var thread = new Thread(() => Application.Run(new GDITest(this)));
                thread.Start();
            }
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
            _gdiControlList.Add(gdiControl);
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

            foreach (var gdiControl in _gdiControlList)
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