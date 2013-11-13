using System.Net.Http;
using NUnit.Framework;
using Should.Fluent;

namespace Simple.Aras.Tests
{
    // ReSharper disable InconsistentNaming
    public class opening_a_connection_should : ActionSpecification<dynamic>
// ReSharper restore InconsistentNaming
    {
        protected override dynamic Given()
        {
            return ArasInnovator.Open(new ArasHttpServerConnection(new HttpClient()));
        }

        [Test]
        public void return_an_aras_innovator_adapter()
        {
            ((object) Sut).Should().Be.OfType<ArasInnovatorAdaptor>();
        }
    }
}