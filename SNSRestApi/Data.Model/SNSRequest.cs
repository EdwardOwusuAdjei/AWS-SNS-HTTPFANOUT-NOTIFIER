using System;

namespace SNSRestApi.Data.Model
{
    /// <summary>
    /// Amazon SNS Request. https://docs.aws.amazon.com/sns/latest/dg/sns-http-https-endpoint-as-subscriber.html
    /// </summary>
    public class SNSRequest
    {
        public string Type { get; set; } 
        public string MessageId { get; set; } 
        public string TopicArn { get; set; } 
        public string Subject { get; set; } 
        public string Message { get; set; } 
        public DateTime Timestamp { get; set; } 
        public string SignatureVersion { get; set; } 
        public string Signature { get; set; } 
        public string SigningCertURL { get; set; } 
        public string UnsubscribeURL { get; set; } 
    }
}