using System;
using System.Threading;
using Newtonsoft.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SNSRestApi.Data.Model;
using SNSRestApi.Data.Model.Configuration;
using SNSRestApi.DependencyResolver;
using SNSRestApi.Observer;
using SNSRestApi.Service.SMSChannel;

namespace SNSRestApi.Service
{
    /// <summary>
    /// Main entry point for all signals
    /// </summary>
    public class Receiver:IReceiver
    {
        private readonly IOptions<ServiceConfiguration> _serviceConfiguration;
        /// <summary>
        /// Ctor
        /// </summary>

        public Receiver()
        {
            var serviceConfiguration =
                ServiceDependency.InternalServiceProvider.GetService<IOptions<ServiceConfiguration>>();
            _serviceConfiguration = serviceConfiguration;
        }
        /// <summary>
        /// Create channel
        /// </summary>
        private readonly Channel<SNSRequest> _sChannel = Channel.CreateUnbounded<SNSRequest>(
            new UnboundedChannelOptions { SingleReader = false, SingleWriter = true });
        /// <summary>
        /// Accept incoming SNS Request
        /// </summary>
        /// <param name="payload">SNS Request Body</param>
        /// <returns></returns>
        public async Task Accept(SNSRequest payload)
        {
            var writer = _sChannel.Writer;
            await writer.WriteAsync(payload);
        }

        /// <summary>
        /// Start Reader with specified workers
        /// </summary>
        /// <returns></returns>
        public async Task<bool> StartReader()
        {
            
            var sChannelReader = _sChannel.Reader;
            for (var i = 0; i < _serviceConfiguration.Value.Workers; i++)
                 Task.Run(() => ListenToChannel(sChannelReader));
            return true;
        }
        
        /// <summary>
        /// Channel listener method
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        
        private static async Task ListenToChannel(ChannelReader<SNSRequest> reader)
        {
            while (await reader.WaitToReadAsync())
            {
                while (reader.TryRead(out var payload))
                {
                    var observable = ServiceDependency.InternalServiceProvider.GetService<IEventObserver>();
                    observable.EventMessage(payload);
                    
                }
            }
        }
        
        /// <summary>
        /// Obj - Accessor
        /// </summary>
        public ChannelReader<SNSRequest> Reader => _sChannel.Reader;
    }
}