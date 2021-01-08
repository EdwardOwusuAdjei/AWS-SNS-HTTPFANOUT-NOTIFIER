namespace SNSRestApi.Data.Model.Configuration
{
    public class Policies
    {
        /// <summary>
        /// Some policies governing service reliability
        /// </summary>
        public string ResiliencePolicy { get; set; }
        public int RetryTimes { get; set; }
    }
}