using System;
using System.Threading.Tasks;
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

            await Task.WhenAll(txServer.Connect(), rxServer.Connect());

            txtMessage.Enabled = true;
            btnSend.Enabled = true;

            await rxServer.Send(""); 

            lstMessages.Items.Add(string.Format(
                "Connected to {0}.", Url));

            while (true)
            {
                ChatMessage msg = await rxServer.Receive<ChatMessage>();
                lstMessages.Items.Add(msg.ToString());
            }
        }
        private void chatFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            txtMessage.Enabled = false;
            btnSend.Enabled = false;

            lstMessages.Items.Add("Disconnecting...");

            //txServer.Close();
            //rxServer.Close();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            txtMessage.Enabled = false;
            btnSend.Enabled = false;

            await txServer.Send<ChatMessage>(new ChatMessage(txtName.Text, txtMessage.Text));

            txtMessage.Text = "";
            txtMessage.Enabled = true;
            btnSend.Enabled = true;
        }       
    }
}
