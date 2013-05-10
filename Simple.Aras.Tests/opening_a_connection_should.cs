using System.Xml.Linq;
using Aras.IOM;
using Moq;
using NUnit.Framework;
using Should.Fluent;

namespace Simple.Aras.Tests
{
    // ReSharper disable InconsistentNaming
    public class opening_a_connection_should : ActionSpecification<dynamic>
// ReSharper restore InconsistentNaming
    {
        private IServerConnection connectionMock;

        protected override void Setup()
        {
            connectionMock = Mock.Of<IServerConnection>();
        }

        protected override dynamic Given()
        {
            return ArasInnovator.Open(connectionMock);
        }

        [Test]
        public void return_an_aras_innovator_adapter()
        {
            ((object) Sut).Should().Be.OfType<ArasInnovatorAdaptor>();
        }
    }
}