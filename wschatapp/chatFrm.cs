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

        private WebSocket txServer, rxServer;

        public chatFrm()
        {
            Url = @"wss://fa-live.herokuapp.com";

            txServer = new WebSocket(Url + "/tx");
            rxServer = new WebSocket(Url + "/rx");

            rxServer.OnMessage += Server_OnMessage;

            InitializeComponent();

            txServer.Connect();

            rxServer.Connect();
            rxServer.Send("");

            if (txServer.IsAlive)
                lstMessages.Items.Add(string.Format(
                    "Connecting to {0}", txServer.Url));

        }

        private void Server_OnMessage(object sender, MessageEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(
                    new Action<object, MessageEventArgs>(Server_OnMessage),
                    sender,
                    e);

                return;
            }

            lstMessages.Items.Add(e.Data);
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            txtMessage.Enabled = false;
            btnSend.Enabled = false;

            txServer.Send(txtMessage.Text);

            txtMessage.Text = "";
            txtMessage.Enabled = true;
            btnSend.Enabled = true;
        }       
    }
}
