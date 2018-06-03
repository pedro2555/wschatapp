using System;
using System.Windows.Forms;

namespace wschatapp
{
    public partial class chatFrm : Form
    {
        string Url;

        private WebSocketWrapper txServer, rxServer;

        public chatFrm()
        {
            Url = @"wss://fa-live.herokuapp.com";

            txServer = new WebSocketWrapper(Url + "/tx");
            rxServer = new WebSocketWrapper(Url + "/rx");

            InitializeComponent();

            Connect();
        }

        private async void Connect()
        {
            lstMessages.Items.Add(string.Format(
                "Connecting to {0}...", Url));

            await txServer.Connect();
            await rxServer.Connect();

            await rxServer.Send("");

            lstMessages.Items.Add(string.Format(
                "Connected to {0}.", Url));

            while (true)
                lstMessages.Items.Add(await rxServer.Receive());
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            txtMessage.Enabled = false;
            btnSend.Enabled = false;

            await txServer.Send(txtMessage.Text);

            txtMessage.Text = "";
            txtMessage.Enabled = true;
            btnSend.Enabled = true;
        }       
    }
}
