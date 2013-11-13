using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using NUnit.Framework;

namespace Simple.Aras.Tests
{
    public class invoking_a_method_should : ActionSpecification<ArasInnovatorMethodAdaptor>
    {
        private readonly Guid aGuid = Guid.NewGuid();
        private string content;

        protected override ArasInnovatorMethodAdaptor Given()
        {
            return new ArasInnovatorMethodAdaptor(new ArasHttpServerConnection(new HttpClient(new FakeHandler(
                new HttpResponseMessage(HttpStatusCode.InternalServerError), 
                request => content = request.Content.ReadAsStringAsync().Result))
            {
                BaseAddress = new Uri("http://fake")
            }));
        }

        protected override Func<ArasInnovatorMethodAdaptor, Task<Unit>> When()
        {
            return async sut => await ((dynamic) sut).Call_some_method(aGuid: aGuid, aString: "Cool Story Bro");
        }
        [Test]
        public void generate_the_correct_aml()
        {
            Assert.AreEqual(XDocument.Parse(String.Format(@"<?xml version=""1.0""?>
<AML>
    <Item type=""Method"" action=""Call_some_method"">
        <aGuid>{0}</aGuid>
        <aString>Cool Story Bro</aString>
    </Item>
</AML>", aGuid.ToString("N").ToUpperInvariant())).ToString(), content);
        }
    }
}