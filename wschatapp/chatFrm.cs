using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wschatapp
{
    public partial class chatFrm : Form
    {
        string Url;

        private WebSocketWrapper server;

        public string Username
        { get; private set; }

        public chatFrm(string username)
        {
            Username = username;
            Text = "Chat App - " + Username;

            Url = @"wss://fa-live.herokuapp.com";

            server = new WebSocketWrapper(Url + "/chat");

            InitializeComponent();

            Connect();
        }

        private async void Connect()
        {
            await server.Connect();

            txtMessage.Enabled = true;
            btnSend.Enabled = true;

            await server.Send<ChatMessage>(new ChatMessage(
                Username,
                "Joined the chat")); 

            while (true)
            {
                ChatMessage msg = await server.Receive<ChatMessage>();
                lstMessages.Items.Add(msg.ToString());
            }
        }
        private void chatFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            txtMessage.Enabled = false;
            btnSend.Enabled = false;

            server.Close();
            server.Close();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            txtMessage.Enabled = false;
            btnSend.Enabled = false;

            await server.Send<ChatMessage>(new ChatMessage(Username, txtMessage.Text));

            txtMessage.Text = "";
            txtMessage.Enabled = true;
            btnSend.Enabled = true;
        }       
    }
}
