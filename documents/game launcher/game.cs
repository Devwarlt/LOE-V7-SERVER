using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Launcher
{
    public partial class Launcher : Form
    {
        public Launcher()
        {
            InitializeComponent();
        }

        private void Launcher_Load(object sender, EventArgs e) { }

        private void webclient_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Start();
        }

        protected readonly string status = "[Game Launcher] Status: ";

        delegate void SetTextCallback(string text);

        private WebBrowser _webclient { get; set; }
        private Label _gameLauncherStatus { get; set; }
        private string _weburl { get; set; }

        protected void Start()
        {
            _webclient = webclient;
            _gameLauncherStatus = gameLauncherStatus;
            _weburl = webclient.Url?.AbsoluteUri;

            if (_weburl != null)
                _weburl = _webclient.Url.AbsoluteUri;

            new Thread(new ThreadStart(LoadFile)).Start();
            //new Thread(new ThreadStart(BeginHandlers)).Start();
        }

        private async void LoadFile()
        {
            UpdateStatus("Loading game...");
            await Task.Delay(1000);
            _webclient.Navigate("http://testing.loesoft.org:1000/v6/testing/resources/LoE.swf");
            //_webclient.Navigate("http://testing.loesoft.org:1000/v6/testing/Desktop?type=application/x-shockwave-flash&data=resources/client-release.swf&width=100%&height=100%&id=loe&style=visibility:%20visible&wmode=direct&quality=high&bgcolor=000000&flashvars=env=loesoft-test&platform=72e02bba3db21fd90adf45c884043cddc080d88c5adf2e158df2d4fb2cfba070&time=1514579244");
            await Task.Delay(1000);
        }

        private async void BeginHandlers()
        {
            try
            {
                do
                {
                    string handlerMessage = string.Empty;
                    byte handlerTypeId = Convert.ToByte(_weburl.Split('&')[1].Split('=')[1]);
                    if (handlers.ContainsKey(handlerTypeId))
                        if (handlers.TryGetValue(handlerTypeId, out handlerMessage) && _gameLauncherStatus.InvokeRequired)
                            UpdateStatus(handlerMessage);
                    await Task.Delay(1000);
                } while (true);
            } catch { return; }
        }

        private void UpdateStatus(string handlerMessage)
        {
            if (_gameLauncherStatus.InvokeRequired)
            {
                try
                {
                    Invoke(new SetTextCallback(UpdateStatus), new object[] { $"{handlerMessage}" });
                } catch (ObjectDisposedException e) {
                    Invoke(new SetTextCallback(UpdateStatus), new object[] { $"{e.Message}" });
                }
            }
            else
                _gameLauncherStatus.Text = $"{status}{handlerMessage}";
        }

        private readonly Dictionary<byte, string> handlers = new Dictionary<byte, string> {
            { 0, "Flash Player not found! You need to download Flash Player to play." },
            { 1, "All plugins installed, game is ready." },
            { 2, "Activate Flash Player plugin in your browser to continue." },
            { 255, null }
        };
    }
}
