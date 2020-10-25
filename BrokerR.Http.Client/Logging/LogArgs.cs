using System;

namespace BrokerR.Http.Client.Logging
{
    public sealed class LogArgs : EventArgs
    {
        public BrokerRLogLevel LogLevel { get; }
        
        public string Message { get; }
        
        public Exception? Exception { get; set; }

        public LogArgs(BrokerRLogLevel logLevel, string message)
        {
            Message = message;
            LogLevel = logLevel;
        }

        public override string ToString()
        {
            if (Exception != null)
                return $"{Message}{Environment.NewLine}{Exception}";

            return Message;
        }
    }
}