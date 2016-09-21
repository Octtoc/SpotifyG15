using LogitechLcdWrapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SpotifyAPI.Local;
using SpotifyAPI.Local.Enums;
using SpotifyAPI.Local.Models;

namespace SpotifyG15
{
    class Program
    {
        static void Main(string[] args)
        {
            SpotifyLocalAPI spotify;
            Bitmap bmp;
            Bitmap vorlage;
            Graphics gfx;

            spotify = new SpotifyLocalAPI();
            if (!SpotifyLocalAPI.IsSpotifyRunning())
                return; //Make sure the spotify client is running
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
                return; //Make sure the WebHelper is running

            if (!spotify.Connect())
                return; //We need to call Connect before fetching infos, this will handle Auth stuff
            
            vorlage = new Bitmap(Image.FromFile(@"C:\Users\Michael\Documents\visual studio 2015\Projects\SpotifyG15\SpotifyG15\bin\Debug\Vorlage.bmp"));
            bmp = new Bitmap(Image.FromFile(@"C:\Users\Michael\Documents\visual studio 2015\Projects\SpotifyG15\SpotifyG15\bin\Debug\Vorlage.bmp"));
            gfx = Graphics.FromImage(bmp);
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            LogitechGSDK.LogiLcdInit("wawd", LogitechGSDK.LOGI_LCD_TYPE_MONO);

            ConsoleKeyInfo keypressed = new ConsoleKeyInfo();
            while (keypressed.Key != ConsoleKey.Escape)
            {
                StatusResponse status = spotify.GetStatus(); //status contains infos

                DateTime currentTime = new DateTime();
                DateTime trackLength = new DateTime();

                double percent = status.PlayingPosition / status.Track.Length;
                int playingPosition = Convert.ToInt32(153 * percent);

                currentTime = currentTime.AddSeconds(status.PlayingPosition);
                trackLength = trackLength.AddSeconds(status.Track.Length);

                //Console.WriteLine(spotify.Status.track.album_resource.name);

                string time = currentTime.Minute.ToString() + " : " + currentTime.Second.ToString() + " / " +
                    trackLength.Minute.ToString() + " : " + trackLength.Second.ToString();

                gfx.DrawString(status.Track.AlbumResource.Name, new Font("Courier", 7), Brushes.Black, 5, 1);
                gfx.DrawString(status.Track.ArtistResource.Name, new Font("Courier", 7), Brushes.Black, 5, 11);
                gfx.DrawString(time, new Font("Courier", 7), Brushes.Black, 5, 21);
                gfx.FillRectangle(Brushes.Black, new Rectangle(3, 36, playingPosition, 5));

                if (LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_MONO_BUTTON_0))
                {
                    Console.WriteLine(spotify.Pause());
                }
                else if (LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_MONO_BUTTON_1))
                {
                    
                }
                else if (LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_MONO_BUTTON_2))
                {
                    Console.WriteLine(spotify.Play());
                }
                else if (LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_MONO_BUTTON_3))
                {

                }
                else
                {

                }

                FillFromBitmap(bmp);
                Thread.Sleep(200);
                gfx.DrawImage(vorlage, new Point(0,0));
                //keypressed = Console.ReadKey();
            }
            LogitechGSDK.LogiLcdShutdown();
        }

        private static void FillFromBitmap(Bitmap bmp)
        {
            int bufferSize = (LogitechGSDK.LOGI_LCD_MONO_WIDTH * LogitechGSDK.LOGI_LCD_MONO_HEIGHT);
            byte[] pixelMatrix = new byte[bufferSize];

            for (int y = 0; y < LogitechGSDK.LOGI_LCD_MONO_HEIGHT; ++y)
            {
                for (int x = 0; x < LogitechGSDK.LOGI_LCD_MONO_WIDTH; ++x)
                {
                    Color trueColor = bmp.GetPixel(x, y);
                    byte nColor = (byte)((trueColor.R == 255 && trueColor.G == 255 && trueColor.B == 255) ? 0 : 255);

                    pixelMatrix[x + (y * 160)] = nColor;
                }
            }
            LogitechGSDK.LogiLcdMonoSetBackground(pixelMatrix);
            LogitechGSDK.LogiLcdUpdate();
        }
    }
}
