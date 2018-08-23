using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EdgeHub;
using Newtonsoft.Json;

namespace Functions.Samples
{
    public static class EdgeHubSamples
    {
        public static async Task FilterMessageAndSendMessage(
                    [EdgeHubTrigger("input1")] Message messageReceived,
                    [EdgeHub(OutputName = "output1")] IAsyncCollector<Message> output)
        {
            const int defaultTemperatureThreshold = 19;
            byte[] messageBytes = messageReceived.GetBytes();
            var messageString = System.Text.Encoding.UTF8.GetString(messageBytes);

            // Get message body, containing the Temperature data         
            var messageBody = JsonConvert.DeserializeObject<MessageBody>(messageString);

            if (messageBody != null && messageBody.Machine.Temperature > defaultTemperatureThreshold)
            {
                var filteredMessage = new Message(messageBytes);
                foreach (KeyValuePair<string, string> prop in messageReceived.Properties)
                {
                    filteredMessage.Properties.Add(prop.Key, prop.Value);
                }

                filteredMessage.Properties.Add("MessageType", "Alert");
                await output.AddAsync(filteredMessage).ConfigureAwait(false);
            }
        }

        public static async Task FilterStringAndSendMessage(
            [EdgeHubTrigger("input1")] string messageReceived,
            [EdgeHub(OutputName = "output1")] IAsyncCollector<Message> output)
        {
            // Get message body, containing the Temperature data         
            var message = JsonConvert.DeserializeObject<MessageBody>(messageReceived);

            const int defaultTemperatureThreshold = 19;
            if (message != null && message.Machine.Temperature > defaultTemperatureThreshold)
            {
                var filteredMessage = new Message(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));

                filteredMessage.Properties.Add("MessageType", "Alert");
                await output.AddAsync(filteredMessage).ConfigureAwait(false);
            }
        }

        public static async Task FilterStringAndSendString(
            [EdgeHubTrigger("input1")] string messageReceived,
            [EdgeHub(OutputName = "output1")] IAsyncCollector<string> output)
        {
            // Get message body, containing the Temperature data         
            var message = JsonConvert.DeserializeObject<MessageBody>(messageReceived);

            const int defaultTemperatureThreshold = 19;
            if (message != null && message.Machine.Temperature > defaultTemperatureThreshold)
            {
                await output.AddAsync(JsonConvert.SerializeObject(message)).ConfigureAwait(false);
            }
        }

        public static async Task FilterStringWithPropertiesAndSendMessage(
            [EdgeHubTrigger("input1")] string messageReceived,
            DateTime EnqueuedTimeUtc,
            ulong SequenceNumber,
            IDictionary<string, string> properties,
            [EdgeHub(OutputName = "output1")] IAsyncCollector<Message> output)
        {
            // Get message body, containing the Temperature data         
            var message = JsonConvert.DeserializeObject<MessageBody>(messageReceived);

            const int defaultTemperatureThreshold = 19;
            if (message != null && message.Machine.Temperature > defaultTemperatureThreshold)
            {
                var filteredMessage = new Message(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));

                filteredMessage.Properties.Add("MessageType", "Alert");
                foreach (var p in properties)
                {
                    filteredMessage.Properties.Add(p.Key, p.Value);
                }
                await output.AddAsync(filteredMessage).ConfigureAwait(false);
            }
        }

        public static async Task FilterMessageAndSendPoco(
            [EdgeHubTrigger("input1")] Message messageReceived,
            [EdgeHub(OutputName = "output1")] IAsyncCollector<MessageBody> output)
        {
            const int defaultTemperatureThreshold = 19;
            byte[] messageBytes = messageReceived.GetBytes();
            var messageString = System.Text.Encoding.UTF8.GetString(messageBytes);

            // Get message body, containing the Temperature data         
            var messageBody = JsonConvert.DeserializeObject<MessageBody>(messageString);

            if (messageBody != null && messageBody.Machine.Temperature > defaultTemperatureThreshold)
            {
                await output.AddAsync(messageBody).ConfigureAwait(false);
            }
        }

        public static async Task FilterBytesAndSendMessage(
            [EdgeHubTrigger("input1")] byte[] messageReceived,
            [EdgeHub(OutputName = "output1")] IAsyncCollector<Message> output)
        {
            // Get message body, containing the Temperature data         
            var message = JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(messageReceived));

            const int defaultTemperatureThreshold = 19;
            if (message != null && message.Machine.Temperature > defaultTemperatureThreshold)
            {
                var filteredMessage = new Message(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));

                filteredMessage.Properties.Add("MessageType", "Alert");
                await output.AddAsync(filteredMessage).ConfigureAwait(false);
            }
        }

        public static async Task FilterBytesAndSendBytes(
            [EdgeHubTrigger("input1")] byte[] messageReceived,
            [EdgeHub(OutputName = "output1")] IAsyncCollector<byte[]> output)
        {
            // Get message body, containing the Temperature data         
            var message = JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(messageReceived));

            const int defaultTemperatureThreshold = 19;
            if (message != null && message.Machine.Temperature > defaultTemperatureThreshold)
            {
                var filteredMessage = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                await output.AddAsync(filteredMessage).ConfigureAwait(false);
            }
        }

        public class MessageBody
        {
            public Machine Machine { get; set; }

            public Ambient Ambient { get; set; }

            public DateTime TimeCreated { get; set; }
        }

        public class Machine
        {
            public double Temperature { get; set; }

            public double Pressure { get; set; }
        }

        public class Ambient
        {
            public double Temperature { get; set; }

            public int Humidity { get; set; }
        }
    }
}
