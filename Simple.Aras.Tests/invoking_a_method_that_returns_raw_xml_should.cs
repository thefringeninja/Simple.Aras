using System;
using System.Xml;
using System.Xml.Serialization;
using Aras.IOM;
using Moq;
using NUnit.Framework;

namespace Simple.Aras.Tests
{
    public class invoking_a_method_that_returns_raw_xml_should
    : ActionSpecification<ArasInnovatorMethodAdaptor, TestItem>
    {
        private IServerConnection serverConnection;
        private Mock<IServerConnection> serverConnectionMock;
        private readonly Guid aGuid = Guid.NewGuid();
        private const string result = @"
<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"">
  <SOAP-ENV:Body>
    <Result>&lt;?xml version=""1.0""?&gt;&lt;TheRoot&gt;&lt;Number&gt;123&lt;/Number&gt;&lt;Description&gt;Interesting&lt;/Description&gt;&lt;SomeOtherItem&gt;&lt;Name&gt;A&lt;/Name&gt;&lt;/SomeOtherItem&gt;&lt;SomeOtherItem&gt;&lt;Name&gt;B&lt;/Name&gt;&lt;/SomeOtherItem&gt;&lt;/TheRoot&gt;</Result>
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
                                .Callback<String, XmlDocument, XmlDocument>((actionName, inDOM, outDOM) => outDOM.LoadXml(result));
        }

        protected override ArasInnovatorMethodAdaptor Given()
        {
            return new ArasInnovatorMethodAdaptor(new Innovator(serverConnection));
        }

        protected override Func<ArasInnovatorMethodAdaptor, TestItem> When()
        {
            return sut => ((dynamic)sut).Call_some_method(aGuid: aGuid, aString: "Cool Story Bro");
        }

        [Test]
        public void deserialize_the_result()
        {
            Assert.NotNull(Result);
            Assert.AreEqual(123, Result.Number);
            Assert.AreEqual("Interesting", Result.Description);
            Assert.AreEqual(2, Result.SomeOtherItems.Length);
            Assert.AreEqual("A", Result.SomeOtherItems[0].Name);
            Assert.AreEqual("B", Result.SomeOtherItems[1].Name);
        }

        [Test]
        public void not_throw_an_exception()
        {
            Assert.IsInstanceOf<NoExceptionThrownException>(ThrownException);
        }
    }
    [XmlRoot("TheRoot")]
    public class TestItem
    {
        public int Number { get; set; }
        public string Description { get; set; }
        [XmlElement("SomeOtherItem")]
        public OtherItem[] SomeOtherItems { get; set; }
    }

    public class OtherItem
    {
        public string Name { get; set; }
    }
}