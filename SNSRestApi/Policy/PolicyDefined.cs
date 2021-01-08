using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using SNSRestApi.Data.Model.Configuration;
using SNSRestApi.DependencyResolver;

namespace SNSRestApi.Policy
{
    public static class PolicyDefined
    {
        /// <summary>
        /// Retry Resilience Policy
        /// </summary>
        /// <returns></returns>
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var retryPolicy = ServiceDependency.InternalServiceProvider
                .GetService<IOptions<Policies>>();
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryPolicy.Value.RetryTimes, 
                    retryAttempt => 
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}