using GDIMusic;
using GDIMusic.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        private readonly string MBeePluginName = "GMediaPlugin";

        private MusicBeeApiInterface _mbApiInterface;
        private PluginInfo about = new PluginInfo();

        private System.Timers.Timer _timer = new System.Timers.Timer();

        private GDIDynamic _gdi;
        private GDIText _artist;
        private GDIText _title;
        private GDIProgressBar _progressLength;
        
        // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        public void Close(PluginCloseReason reason)
        {
        }

        public bool Configure(IntPtr panelHandle)
        {
            // save any persistent settings in a sub-folder of this path
            string dataPath = _mbApiInterface.Setting_GetPersistentStoragePath();

            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window
            if (panelHandle != IntPtr.Zero)
            {
                Panel configPanel = (Panel)Panel.FromHandle(panelHandle);
                Label prompt = new Label();
                prompt.AutoSize = true;
                prompt.Location = new Point(0, 0);
                prompt.Text = "prompt:";
                TextBox textBox = new TextBox();
                textBox.Bounds = new Rectangle(60, 0, 100, textBox.Height);
                configPanel.Controls.AddRange(new Control[] { prompt, textBox });
            }
            return false;
        }

        // return an array of lyric or artwork provider names this plugin supports
        // the providers will be iterated through one by one and passed to the RetrieveLyrics/ RetrieveArtwork function in order set by the user in the MusicBee Tags(2) preferences screen until a match is found
        public string[] GetProviders()
        {
            return null;
        }

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            _mbApiInterface = new MusicBeeApiInterface();
            _mbApiInterface.Initialise(apiInterfacePtr);
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = MBeePluginName;
            about.Description = MBeePluginName;
            about.Author = "Michael Bubeck";
            about.TargetApplication = "";   // current only applies to artwork, lyrics or instant messenger name that appears in the provider drop down selector or target Instant Messenger
            about.Type = PluginType.General;
            about.VersionMajor = 0;  // your plugin version
            about.VersionMinor = 0;
            about.Revision = 1;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            about.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function

            _gdi = new GDIDynamic(new Bitmap(160, 43));

            _title = new GDIText(new Rectangle(10, 3, 10, 10), "");
            _artist = new GDIText(new Rectangle(10, 12, 0, 0), "");
            _progressLength = new GDIProgressBar(new Rectangle(10, 35, 140, 5));

            _gdi.AddControl(_title);
            _gdi.AddControl(_artist);
            _gdi.AddControl(_progressLength);

            _timer.Elapsed += Timer_Elapsed;
            _timer.Interval = 100;
            _timer.Enabled = true;
            _timer.Start();

            return about;
        }

        // receive event notifications from MusicBee
        // you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            // perform some action depending on the notification type
            switch (type)
            {
                case NotificationType.PluginStartup:

                    // perform startup initialisation
                    switch (_mbApiInterface.Player_GetPlayState())
                    {
                        case PlayState.Playing:

                        case PlayState.Paused:

                            // ...
                            break;
                    }
                    break;
                case NotificationType.TrackChanged:
                    _artist.Text = _mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Artist);
                    _title.Text = _mbApiInterface.NowPlaying_GetFileTag(MetaDataType.TrackTitle);
                    _progressLength.Max = _mbApiInterface.NowPlaying_GetDuration();

                    // ...
                    break;
            }
        }

        // return Base64 string representation of the artwork binary data from the requested provider
        // only required if PluginType = ArtworkRetrieval
        // return null if no artwork is found
        public string RetrieveArtwork(string sourceFileUrl, string albumArtist, string album, string provider)
        {
            //Return Convert.ToBase64String(artworkBinaryData)
            return null;
        }

        // return lyrics for the requested artist/title from the requested provider
        // only required if PluginType = LyricsRetrieval
        // return null if no lyrics are found
        public string RetrieveLyrics(string sourceFileUrl, string artist, string trackTitle, string album, bool synchronisedPreferred, string provider)
        {
            return null;
        }

        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // its up to you to figure out whether anything has changed and needs updating
        public void SaveSettings()
        {
            // save any persistent settings in a sub-folder of this path
            string dataPath = _mbApiInterface.Setting_GetPersistentStoragePath();
        }

        // uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _progressLength.Progress = _mbApiInterface.Player_GetPosition();
        }
    }
}