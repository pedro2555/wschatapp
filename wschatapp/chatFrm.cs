using System.Windows.Forms;
using WebSocketSharp;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace wschatapp
{
    public partial class chatFrm : Form
    {
        string Url;

        private WebSocket server;

        public chatFrm()
        {
            //Url = @"wss://echo.websocket.org";
            server = new WebSocket(@"wss://fa-live.herokuapp.com/echo");

            server.OnMessage += Server_OnMessage;

            InitializeComponent();

            server.Connect();

            if (server.ReadyState == WebSocketState.Connecting)
                lstMessages.Items.Add(string.Format(
                    "Connecting to {0}", Url));

        }

        private void Server_OnMessage(object sender, MessageEventArgs e)
        {
            while (!Disposing)
            {
                if (server.ReadyState != WebSocketState.Open)
                {
                    Task.Delay(1000);
                    continue;
                }
                try
                {
                    string message = e.Data;

                    lstMessages.Items.Add(message);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            txtMessage.Enabled = false;
            btnSend.Enabled = false;

            server.Send(txtMessage.Text);

            txtMessage.Text = "";
            txtMessage.Enabled = true;
            btnSend.Enabled = true;
        }       
    }
}
