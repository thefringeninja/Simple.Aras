using System;
using System.Xml;
using Aras.IOM;
using Moq;
using NUnit.Framework;

namespace Simple.Aras.Tests
{
    public class invoking_a_method_that_returns_a_fault_should 
        : ActionSpecification<ArasInnovatorMethodAdaptor>
    {
        private IServerConnection serverConnection;
        private Mock<IServerConnection> serverConnectionMock;
        private readonly Guid aGuid = Guid.NewGuid();
        private const string fault = @"
<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"">
  <SOAP-ENV:Body>
    <SOAP-ENV:Fault xmlns:af=""http://www.aras.com/InnovatorFault"">
      <faultcode>SOAP-ENV:Server.SomethingWentHorriblyWrongException</faultcode>
      <faultstring><![CDATA[Something went horribly wrong.]]></faultstring>
      <detail>
        <af:legacy_detail><![CDATA[Something went horribly wrong.]]></af:legacy_detail>
        <af:exception message=""Something went horribly wrong."" type=""Aras.Server.Core.SomethingWentHorriblyWrongException"" />
      </detail>
    </SOAP-ENV:Fault>
  </SOAP-ENV:Body>
</SOAP-ENV:Envelope>
";
        protected override void Setup()
        {
            serverConnection = Mock.Of<IServerConnection>();
            serverConnectionMock = Mock.Get(serverConnection);
            serverConnectionMock.Setup(connection =>
                                       connection.CallAction(
                                           It.IsAny<String>(),
                                           It.IsAny<XmlDocument>(),
                                           It.IsAny<XmlDocument>()))
                                .Callback<String, XmlDocument, XmlDocument>((actionName, inDOM, outDOM) => outDOM.LoadXml(fault));
        }

        protected override ArasInnovatorMethodAdaptor Given()
        {
            return new ArasInnovatorMethodAdaptor(new Innovator(serverConnection));
        }

        protected override Func<ArasInnovatorMethodAdaptor, Unit> When()
        {
            return sut => ((dynamic) sut).Call_some_method(aGuid: aGuid, aString: "Cool Story Bro");
        }

        [Test]
        public void throws_invalid_operation_exception()
        {
            Assert.IsInstanceOf<InvalidOperationException>(ThrownException);
        }

        [Test]
        public void exception_indicates_what_went_wrong()
        {
            Assert.AreEqual("Something went horribly wrong.", ThrownException.Message);
        }

    }
}