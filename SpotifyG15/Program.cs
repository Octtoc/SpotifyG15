using GDIMusic;
using GDIMusic.Controls;
using LogitechLcdWrapper;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Enums;
using SpotifyAPI.Local.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyG15
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SpotifyLocalAPI spotify;
            GDIDynamic gfx = new GDIDynamic(new Bitmap(160, 43));
            GDIText gdiTitle = new GDIText(new Rectangle(5, 1, 0, 0), "");
            GDIText gdiTrack = new GDIText(new Rectangle(5, 11, 0, 0), "");
            GDIText gdiTime = new GDIText(new Rectangle(5, 21, 0, 0), "");
            GDIProgressBar gdiProgress = new GDIProgressBar(new Rectangle(3, 36, 140, 5));

            gfx.AddControl(gdiTitle);
            gfx.AddControl(gdiTrack);
            gfx.AddControl(gdiTime);
            gfx.AddControl(gdiProgress);

            spotify = new SpotifyLocalAPI();
            if (!SpotifyLocalAPI.IsSpotifyRunning())
                return; //Make sure the spotify client is running
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
                return; //Make sure the WebHelper is running

            if (!spotify.Connect())
                return; //We need to call Connect before fetching infos, this will handle Auth stuff

            ConsoleKeyInfo keypressed = new ConsoleKeyInfo();
            while (keypressed.Key != ConsoleKey.Escape)
            {
                StatusResponse status = spotify.GetStatus(); //status contains infos

                DateTime currentTime = new DateTime();
                DateTime trackLength = new DateTime();

                currentTime = currentTime.AddSeconds(status.PlayingPosition);
                trackLength = trackLength.AddSeconds(status.Track.Length);

                //Console.WriteLine(spotify.Status.track.album_resource.name);

                string time = currentTime.Minute.ToString() + " : " + currentTime.Second.ToString() + " / " +
                    trackLength.Minute.ToString() + " : " + trackLength.Second.ToString();

                gdiTitle.Text = status.Track.TrackResource.Name;
                gdiTrack.Text = status.Track.ArtistResource.Name;
                gdiTime.Text = time;
                gdiProgress.Max = status.Track.Length;
                gdiProgress.Progress = Convert.ToInt32((status.PlayingPosition));

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

                Thread.Sleep(200);
            }
            gfx.CloseLcd();
        }
    }
}