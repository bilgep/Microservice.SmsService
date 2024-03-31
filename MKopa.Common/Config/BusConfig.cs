namespace MKopa.Common.Config
{
    public class BusConfig
    {
        public string Host { get; set; } = "rabbitmq";
        public string VirtualHost { get; set; } = "/";
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string SendCommandQueue { get; set; } = "send_sms_commands";
        public string SmsSentEventQueueUri { get; set; } = "queue:sms_sent_events";
        public int DelaySeconds { get; set; } = 5;
        public bool IsCommandProducer { get; set; } = false;
        public bool IsEventProducer { get; set; } = false;
        public bool IsCommandConsumer { get; set; } = false;
    }
}
