using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Simple.Aras
{
    public class ArasHttpServerConnection : IArasHttpServerConnection, IDisposable
    {
        private const string DatabasePrefix = "database=";
        private static readonly XNamespace SoapNs;
        private readonly HttpClient client;
        private readonly bool ownsClient;

        private readonly HashAlgorithm hash = MD5.Create();

        static ArasHttpServerConnection()
        {
            SoapNs = "http://schemas.xmlsoap.org/soap/envelope/";
        }

        public ArasHttpServerConnection(HttpClient client)
        {
            this.client = client;
        }

        public ArasHttpServerConnection(Uri connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            var builder = new UriBuilder(connection);
            var pathParts = builder.Path.Split(';');
            if (pathParts.Length != 2 || false == pathParts[1].StartsWith(DatabasePrefix))
                throw new ArgumentException(
                    "Expected /{Path};database={database}, received " + builder.Path + " instead.");

            var database = pathParts[1].Remove(0, DatabasePrefix.Length);

            builder.Path = pathParts[0] + "/Server/InnovatorServer.aspx";

            client = new HttpClient {BaseAddress = builder.Uri};
            client.DefaultRequestHeaders.ExpectContinue = true;
            client.DefaultRequestHeaders.Add("AUTHUSER", builder.UserName);
            client.DefaultRequestHeaders.Add("AUTHPASSWORD", EncodePassword(builder.Password));
            client.DefaultRequestHeaders.Add("DATABASE", database.Replace(" ", "%20"));

            ownsClient = true;
        }

        #region IDisposable Members

        public void Dispose()
        {
            hash.Dispose();
            if (ownsClient) client.Dispose();
        }

        #endregion

        #region IArasHttpServerConnection Members

        public async Task<XElement> ApplyAmlAsync(XElement item, string action = "ApplyAML")
        {
            var requestDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("AML", item));
            using (var result = await client.SendAsync(
                new HttpRequestMessage
                {
                    Content = new StringContent(requestDocument.ToString(), Encoding.UTF8, "text/xml"),
                    Method = new HttpMethod("POST"),
                    Version = new Version(1, 1),
                    Headers =
                    {
                        {"SOAPAction", action}
                    }
                }))
            {
                var responseText = await result.Content.ReadAsStringAsync();

                var responseDocument = XDocument.Parse(responseText);

                var fault = responseDocument.Descendants(SoapNs + "Fault").FirstOrDefault();
                if (fault != null)
                {
                    var faultCode = (from f in fault.Descendants("faultstring")
                                     select f.Value).FirstOrDefault();

                    throw new InvalidOperationException(faultCode ?? "Unknown error.");
                }

                return responseDocument.Descendants("Result").FirstOrDefault();
            }
        }

        #endregion

        private string EncodePassword(string password)
        {
            var buffer = Encoding.ASCII.GetBytes(password);

            return String.Join("", hash.ComputeHash(buffer).Select(b => b.ToString("x")));
        }
    }
}
