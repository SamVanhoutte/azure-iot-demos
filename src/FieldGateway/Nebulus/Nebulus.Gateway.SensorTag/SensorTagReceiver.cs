using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using Nebulus.IoT.Gateway.Core.Channel;
using Nebulus.IoT.Gateway.Core.Channel.Receiver;
using Nebulus.IoT.Gateway.Utilities.Extensions;
using Newtonsoft.Json.Linq;

namespace Nebulus.Gateway.SensorTag
{
    public class SensorTagReceiver : PollingTemplate<string>, IReceiver
    {
        private int portNumber = 9000;
        private HttpClient httpClient;

        public SensorTagReceiver()
        {
            httpClient = new HttpClient();

        }

        public void Configure(IDictionary<string, object> properties)
        {
            double milliseconds = Convert.ToDouble(properties.AsString("PollingInterval", false, "500"));
            portNumber = properties.AsInteger("PortNumber", false, 9000);
            base.PollingInterval = TimeSpan.FromMilliseconds(milliseconds);
        }

        public void StartReceiving(Action<InternalMessage> callback, CancellationToken cancellation)
        {
            base.StartPolling(callback, cancellation);
        }

        protected override IEnumerable<string> PollForMessages()
        {
            var response = httpClient.GetAsync($"http://localhost:{portNumber}/api/sensortag").Result;
            yield return response.Content.ReadAsStringAsync().Result;
        }

        protected override void MessageReceived(string message, Action<InternalMessage> callback)
        {
            var messageStream = new MemoryStream(Encoding.UTF8.GetBytes(message));
            callback(new InternalMessage { Body = messageStream });
        }
    }
}
