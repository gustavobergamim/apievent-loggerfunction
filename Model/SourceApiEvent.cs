using System;

namespace Bergamim.Function.Model
{
    public class SourceApiEvent
    {
        public string Id { get; set; }
        public DateTime EventTime { get; set; }
        public string ServiceName { get; set; }
        public Guid RequestId { get; set; }
        public string RequestIp { get; set; }
        public string OperationName { get; set; }
        public DateTime EventProcessedUtcTime { get; set; }
    }
}