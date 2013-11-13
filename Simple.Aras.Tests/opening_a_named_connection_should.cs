using System;
using NUnit.Framework;
using Should.Fluent;

namespace Simple.Aras.Tests
{
    // ReSharper disable InconsistentNaming
    public class opening_a_named_connection_should : ActionSpecification<dynamic>
// ReSharper restore InconsistentNaming
    {
        protected override void Setup()
        {
        }

        protected override dynamic Given()
        {
            return ArasInnovator.Open(new ArasHttpServerConnection(new Uri("http://user:password@localhost/InnovatorServer;database=development")));
        }

        [Test]
        public void return_an_aras_innovator_adapter()
        {
            ((object) Sut).Should().Be.OfType<ArasInnovatorAdaptor>();
        }
    }
}