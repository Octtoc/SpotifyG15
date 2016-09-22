using GDIMusic;
using GDIMusic.Controls;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace SpotifyG15
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GDIDynamic gfx = new GDIMusic.GDIDynamic(new Bitmap(160, 43));
            SpotifyLocalAPI spotify = new SpotifyLocalAPI();

            GDIText gdiTitle = new GDIText(new Rectangle(5, 1, 0, 0), "");
            GDIText gdiTrack = new GDIText(new Rectangle(5, 11, 0, 0), "");
            GDIText gdiTime = new GDIText(new Rectangle(5, 21, 0, 0), "");
            GDIProgressBar gdiProgress = new GDIProgressBar(new Rectangle(3, 36, 140, 5));

            gfx.AddControl(gdiTitle);
            gfx.AddControl(gdiTrack);
            gfx.AddControl(gdiTime);
            gfx.AddControl(gdiProgress);

            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                Console.WriteLine("Spotify is not running");
                Thread.Sleep(1000);
                return;
            }
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
            {
                Console.WriteLine("Spotify is not running");
                Thread.Sleep(1000);
                return;
            }

            if (!spotify.Connect())
            {
                Console.WriteLine("cannot connect with spotify");
                Thread.Sleep(1000);
                return;
            }

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

                Thread.Sleep(200);
            }
            gfx.CloseLcd();
        }
    }
}