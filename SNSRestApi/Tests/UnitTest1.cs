using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SNSRestApi.Data.Model;
using SNSRestApi.Service;

namespace Tests
{
    public class Tests
    {
        //TODO:: Add more test scenarios
        [SetUp]
        public void Setup()
        {
          
        }

        [Test]
        public async Task TestChannelReceiver()
        {
            var receiver = new Receiver();
            await receiver.StartReader();
            await receiver.Accept(new SNSRequest());
            var result = await receiver.Reader.ReadAsync();
            Assert.AreNotEqual(result,new SNSRequest());
        }
    }
}