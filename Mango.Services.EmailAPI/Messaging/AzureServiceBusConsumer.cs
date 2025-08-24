using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Services;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer:IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly string registerUserQueue;

        private readonly IConfiguration configuration;
        private readonly EmailService emailService;

        private ServiceBusProcessor emailCartProcessor;
        private ServiceBusProcessor registerUserProcessor;

        public AzureServiceBusConsumer(IConfiguration _configuration,EmailService _emailService)
        {
            configuration = _configuration;
            emailService = _emailService;

            serviceBusConnectionString = configuration.GetValue<string>("ServiceBusConnectionString");
            emailCartQueue = configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            registerUserQueue = configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);
            emailCartProcessor = client.CreateProcessor(emailCartQueue);
            registerUserProcessor = client.CreateProcessor(registerUserQueue);

        }

        public async Task Start()
        {
            emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await emailCartProcessor.StartProcessingAsync();

            registerUserProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
            registerUserProcessor.ProcessErrorAsync += ErrorHandler;
            await registerUserProcessor.StartProcessingAsync();

        }

        public async Task Stop()
        {
            await emailCartProcessor.StopProcessingAsync();
            await emailCartProcessor.DisposeAsync();

            await registerUserProcessor.StopProcessingAsync();
            await registerUserProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return  Task.CompletedTask;
        }

        private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            string email = JsonConvert.DeserializeObject<string>(body);

            try
            {
                await emailService.RegisterEmailAndLog(email);
                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message; 
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);

            try
            {
                await emailService.EmailCartAndLog(objMessage);
                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
