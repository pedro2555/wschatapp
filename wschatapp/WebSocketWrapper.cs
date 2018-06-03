using System;
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
        private TaskCompletionSource<string> receiveTask;

        public WebSocketWrapper(string url)
        {
            Url = url;

            ws = new WebSocket(Url);

            ws.OnClose += Ws_OnClose;
            ws.OnOpen += Ws_OnOpen;
            ws.OnError += Ws_OnError;
            ws.OnMessage += Ws_OnMessage;
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            if (receiveTask != null &&
                receiveTask.Task.Status == TaskStatus.WaitingForActivation)
                receiveTask.SetResult(e.Data);
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            if (connectTask != null &&
                connectTask.Task.Status == TaskStatus.WaitingForActivation)
                connectTask.SetException(e.Exception);

            if (receiveTask != null &&
                receiveTask.Task.Status == TaskStatus.WaitingForActivation)
                receiveTask.SetException(e.Exception);
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            if (connectTask != null &&
                connectTask.Task.Status == TaskStatus.WaitingForActivation)
                connectTask.SetResult(null);
        }

        private async void Ws_OnClose(object sender, CloseEventArgs e)
        {
            if (e.WasClean)
                return;

            await Connect();
        }

        public async Task Connect()
        {
            connectTask = new TaskCompletionSource<object>();

            ws.Connect();

            await connectTask.Task;
        }

        public void Close()
        {
            ws.CloseAsync();
        }

        public async Task Send(string data)
        {
            if (connectTask != null &&
                connectTask.Task.Status == TaskStatus.WaitingForActivation)
                await connectTask.Task;
            
            await Task.Run(new Action(() => { ws.Send(data); }));
        }

        public async Task<string> Receive()
        {
            if (connectTask != null &&
                connectTask.Task.Status == TaskStatus.WaitingForActivation)
                await connectTask.Task;

            receiveTask = new TaskCompletionSource<string>();

            return await receiveTask.Task;
        }
    }
}
