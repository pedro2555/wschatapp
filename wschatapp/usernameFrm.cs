using System;
using System.Windows.Forms;

namespace wschatapp
{
    public partial class usernameFrm : Form
    {
        public string Username
        { get { return txtUsername.Text; } }

        public usernameFrm()
        {
            InitializeComponent();
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtUsername.Text == "Choose a username")
                return;

            chatFrm chat = new chatFrm(txtUsername.Text);

            chat.Show();
            Hide();
        }
    }
}
