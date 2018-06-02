using System.Windows.Forms;
using System.Net.WebSockets;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace wschatapp
{
    public partial class chatFrm : Form
    {
        string Url;

        ClientWebSocket server;

        public chatFrm()
        {
            //Url = @"wss://echo.websocket.org";
            Url = @"wss://fa-live.herokuapp.com/echo";
            //Url = @"ws://localhost:8000/echo";
            server = new ClientWebSocket();

            InitializeComponent();

            Connect();

            if (server.State == WebSocketState.Connecting)
                lstMessages.Items.Add(string.Format(
                    "Connecting to {0}", Url));

            AsyncReceiveLoop();
        }

        private async void Connect()
        {
            await server.ConnectAsync(new Uri(Url), CancellationToken.None);
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            txtMessage.Enabled = false;
            btnSend.Enabled = false;

            await server.SendAsync(txtMessage.Text);

            txtMessage.Text = "";
            txtMessage.Enabled = true;
            btnSend.Enabled = true;
        }

        private async void AsyncReceiveLoop()
        {
            while (!Disposing)
            {
                if (server.State != WebSocketState.Open)
                {
                    await Task.Delay(1000);
                    continue;
                }
                try
                {
                    string message = await server.ReceiveAsync();

                    lstMessages.Items.Add(message);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
