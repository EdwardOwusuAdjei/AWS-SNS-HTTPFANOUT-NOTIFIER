using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SNSRestApi.Data.Model;
using SNSRestApi.Data.Model.Configuration;
using SNSRestApi.DependencyResolver;
using SNSRestApi.Service;

namespace SNSRestApi
{
    /// <summary>
    /// Start
    /// </summary>
    class Program
    {
        
        /// <summary>
        /// Entry Point for initialization and route registering
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            var app = WebApplication.Create(args);
            ServiceDependency.Init();
            var receiver = ServiceDependency.InternalServiceProvider.GetService<IReceiver>();
            await receiver.StartReader();
            
            app.MapPost("/receiver", async http =>
            {
                var request = await http.Request.ReadJsonAsync<SNSRequest>();
                await receiver.Accept(request);
                http.Response.StatusCode = StatusCodes.Status200OK;
            });
            
            
            await app.RunAsync();
        }
    }
}
