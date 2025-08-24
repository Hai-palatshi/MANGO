using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private string connectionString = "you connectiom here";
        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(topic_queue_Name);

            var JsonMessage = JsonConvert.SerializeObject(message);
            var jsonByts = Encoding.UTF8.GetBytes(JsonMessage);

            ServiceBusMessage finalMessage = new ServiceBusMessage(jsonByts);
            finalMessage.CorrelationId = Guid.NewGuid().ToString();

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
