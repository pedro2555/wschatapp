using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace wschatapp
{
    public class WebSocketWrapper
    {
        public string Url
        { get; private set; }

        private WebSocket ws;

        private TaskCompletionSource<object> connectTask;

        public event EventHandler<string> OnMessage;

        public WebSocketWrapper(string url)
        {
            Url = url;

            connectTask = new TaskCompletionSource<object>();

            ws = new WebSocket(Url);

            ws.OnClose += Ws_OnClose;
            ws.OnOpen += Ws_OnOpen;
            ws.OnError += Ws_OnError;
            ws.OnMessage += Ws_OnMessage;
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            OnMessage(sender, e.Data);
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            if (connectTask.Task.Status == TaskStatus.Running)
                connectTask.SetException(e.Exception);
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            if (connectTask.Task.Status == TaskStatus.WaitingForActivation)
                connectTask.SetResult(null);
        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            WebSocket _ws = (WebSocket)sender;
            _ws.Connect();
        }

        public async Task Connect()
        {
            ws.Connect();

            await connectTask.Task;
        }

        public async Task Send(string data)
        {
            await Task.Run(new Action(() => { ws.Send(data); }));
        }
    }
}
