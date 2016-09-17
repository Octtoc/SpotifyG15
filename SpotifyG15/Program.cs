using LogitechLcdWrapper;
using SpotifyApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyG15
{
    class Program
    {
        static void Main(string[] args)
        {
            Spotify spotify = new Spotify(Spotify.GetOAuth());
            Responses.CFID cfid = spotify.CFID;

            LogitechGSDK.LogiLcdInit("wawd", LogitechGSDK.LOGI_LCD_TYPE_MONO);

            int bufferSize = (LogitechGSDK.LOGI_LCD_MONO_WIDTH * LogitechGSDK.LOGI_LCD_MONO_HEIGHT) - 1;
            byte[] pixelMatrix = new byte[bufferSize];
            Random rnd = new Random();
            for (int i = 0; i < bufferSize; ++i)
            {
                byte fillByte;
                if (rnd.Next(0, 2) == 0)
                {
                    fillByte = 0x00;
                }
                else
                {
                    fillByte = 0xFF;
                }
                pixelMatrix[i] = fillByte;
            }
            //fill this array with your image
            LogitechGSDK.LogiLcdMonoSetBackground(pixelMatrix);
            LogitechGSDK.LogiLcdUpdate();
            Thread.Sleep(2000);

            ConsoleKeyInfo keypressed = new ConsoleKeyInfo();
            while (keypressed.Key != ConsoleKey.Escape)
            {
                Console.WriteLine(spotify.Status.track.album_resource.name);
                LogitechGSDK.LogiLcdMonoSetText(0, spotify.Status.track.album_resource.name);
                LogitechGSDK.LogiLcdUpdate();
                
                if(LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_MONO_BUTTON_0))
                {
                    Console.WriteLine(spotify.Pause);
                }
                else if (LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_MONO_BUTTON_1))
                {
                    
                }
                else if (LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_MONO_BUTTON_2))
                {
                    Console.WriteLine(spotify.Resume);
                }
                else if (LogitechGSDK.LogiLcdIsButtonPressed(LogitechGSDK.LOGI_LCD_MONO_BUTTON_3))
                {

                }
                else
                {

                }

                Thread.Sleep(500);
                //keypressed = Console.ReadKey();
            }
            LogitechGSDK.LogiLcdShutdown();
        }
    }
}
