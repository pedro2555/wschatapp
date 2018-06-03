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

            if (server.IsAlive)
                lstMessages.Items.Add(string.Format(
                    "Connecting to {0}", server.Url));

        }

        private void Server_OnMessage(object sender, MessageEventArgs e)
        {
            lstMessages.Items.Add(e.Data);
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
