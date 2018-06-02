using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace wschatapp
{
    public static class ClientWebSocketExtensions
    {
        public static async Task SendAsync(this ClientWebSocket ws, string data)
        {
            if (ws.State != WebSocketState.Open)
                return;

            byte[] buff = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

            await ws.SendAsync(
                new ArraySegment<byte>(buff),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }

        public static async Task<string> ReceiveAsync(this ClientWebSocket ws)
        {
            byte[] buff = new byte[1024];
            if (ws.State != WebSocketState.Open)
                return null;

            var result = await ws.ReceiveAsync(
                new ArraySegment<byte>(buff),
                CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await ws.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    string.Empty,
                    CancellationToken.None);
            }
            else
            {
                string data = JsonConvert.DeserializeObject<string>(
                    Encoding.UTF8.GetString(buff).TrimEnd('\0'));

                return JsonConvert.SerializeObject(data);
            }

            return null;
        }
    }
}
