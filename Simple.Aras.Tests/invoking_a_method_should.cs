using System;
using System.Xml;
using Aras.IOM;
using Moq;
using NUnit.Framework;

namespace Simple.Aras.Tests
{
    public class invoking_a_method_should : ActionSpecification<ArasInnovatorMethodAdaptor>
    {
        private IServerConnection serverConnection;
        private Mock<IServerConnection> serverConnectionMock;
        private readonly Guid aGuid = Guid.NewGuid();

        protected override void Setup()
        {
            serverConnection = Mock.Of<IServerConnection>();
            serverConnectionMock = Mock.Get(serverConnection);
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
        public void call_apply_method()
        {
            serverConnectionMock.Verify(
                connection => connection.CallAction(
                    It.Is<String>(action => action == "ApplyMethod"),
                    It.IsAny<XmlDocument>(),
                    It.IsAny<XmlDocument>()));
        }

        [Test]
        public void generate_the_correct_aml()
        {
            serverConnectionMock.Verify(
                connection => connection.CallAction(
                    It.IsAny<String>(),
                    It.Is<XmlDocument>(
                        doc => doc.InnerXml == string.Format(@"<Item type=""Method"" action=""Call_some_method""><aGuid>{0}</aGuid><aString>Cool Story Bro</aString></Item>", aGuid.ToString("N").ToUpper())),
                    It.IsAny<XmlDocument>()));
        }
    }
}