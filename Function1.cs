using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp5
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Function1))]
        public async Task Run(
            [ServiceBusTrigger("reporting", Connection = "ServiceBusConnection")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            // Write Code to Call the power BI API here
            var client = new HttpClient();
            //var resp = await client.GetAsync("https://func-reporting-dev.azurewebsites.net/api/function");
            //string data =  await resp.Content.ReadAsStringAsync();
            
            try
            {
                var resp = await client.GetAsync("https://reporting-dev01.azure-api.net/limit");
                string data = await resp.Content.ReadAsStringAsync();
                _logger.LogInformation("RespData1: {data}", data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception1 {data}", ex.Message);
            }
            
            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
