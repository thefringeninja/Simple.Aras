using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Aras.Tests
{
    public class FakeHandler : DelegatingHandler
    {
        private readonly HttpResponseMessage response;
        private readonly Action<HttpRequestMessage> onSend;

        public FakeHandler(HttpResponseMessage response, Action<HttpRequestMessage> onSend)
        {
            this.response = response;
            this.onSend = onSend;
            InnerHandler = new HttpClientHandler();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            onSend(request);
            return Task.FromResult(response);
        }
    }
}