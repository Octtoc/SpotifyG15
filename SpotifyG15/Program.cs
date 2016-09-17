using SpotifyApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyG15
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Spotify spotify = new Spotify(Spotify.GetOAuth());
            Responses.CFID cfid = spotify.CFID;
            string value = "";

            while(value != "q")
            {
                try
                {
                    Console.WriteLine(spotify.Status.track.album_resource.name);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
                value = Console.ReadLine();
            }
            */
            // LCD structures needed
            LgLcd.NET.LgLcd.lgLcdConnectContext conn = new LgLcd.NET.LgLcd.lgLcdConnectContext();
            LgLcd.NET.LgLcd.lgLcdOpenContext opn = new LgLcd.NET.LgLcd.lgLcdOpenContext();
            LgLcd.NET.LgLcd.lgLcdBitmap160x43x1 bmp = new LgLcd.NET.LgLcd.lgLcdBitmap160x43x1();

            // .NET drawing objects
            Bitmap bmpNet;
            Graphics gfx;

            // Initialize LCD access            
            LgLcd.NET.LgLcd.lgLcdInit();
            conn.appFriendlyName = "LOLO";
            conn.isAutostartable = false;
            conn.isPersistent = false;
            conn.connection = LgLcd.NET.LgLcd.LGLCD_INVALID_CONNECTION;
            LgLcd.NET.LgLcd.lgLcdConnect(ref conn);
            opn.connection = conn.connection;
            opn.index = 1;
            LgLcd.NET.LgLcd.lgLcdOpen(ref opn);

            // Graphics object
            bmpNet = new Bitmap(160, 43);
            gfx = Graphics.FromImage(bmpNet);

            // Clear the screen            
            gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, 160, 43));

            // Drawing goes here.  We can do any .NET graphics object drawing we want.
            // (fonts or whatever.)
            gfx.DrawString("Hello World", new Font("Courier", 8), Brushes.Black, 10, 10);

            // Convert our .NET bitmap to the format that LCD wants
            // (treat anything non-white to be black)
            // This is *not* the most efficient way to do this.
            bmp.hdr = new LgLcd.NET.LgLcd.lgLcdBitmapHeader();
            bmp.hdr.Format = LgLcd.NET.LgLcd.LGLCD_BMP_FORMAT_160x43x1;
            bmp.pixels = new byte[6880];
            for (int y = 0; y < 43; y++)
            {
                for (int x = 0; x < 160; x++)
                {
                    Color trueColor = bmpNet.GetPixel(x, y);
                    byte nColor = (byte)((trueColor.R == 255 && trueColor.G == 255 && trueColor.B == 255) ? 0 : 255);

                    bmp.pixels[y * 160 + x] = nColor;
                }
            }

            // Draw the picture.
            LgLcd.NET.LgLcd.lgLcdUpdateBitmap(opn.device, ref bmp, LgLcd.NET.LgLcd.LGLCD_PRIORITY_NORMAL);

            // Wait for a keypress
            Console.ReadKey();

            // Shut it all down
            LgLcd.NET.LgLcd.lgLcdClose(opn.device);
            LgLcd.NET.LgLcd.lgLcdDisconnect(conn.connection);
            LgLcd.NET.LgLcd.lgLcdDeInit();
        }
    }
}
