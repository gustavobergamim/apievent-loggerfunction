using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bergamim.Function.Model;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bergamim.Function
{
    public static class LoggerSubscriber
    {
        [FunctionName("LoggerSubscriber")]
        public static async Task Run(
            [EventHubTrigger("apievent-sourceapi-events", Connection = "apieventeventhub_loggerfunctionreader_EVENTHUB")] EventData[] events,
            [CosmosDB("apievent-db", "sourceapievents", ConnectionStringSetting = "CosmosDbConnString")] IAsyncCollector<SourceApiEvent> sourceApiEvents,
            ILogger log)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                    var sourceApiEvent = JsonConvert.DeserializeObject<SourceApiEvent>(messageBody);
                    await sourceApiEvents.AddAsync(sourceApiEvent);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }
            
            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
