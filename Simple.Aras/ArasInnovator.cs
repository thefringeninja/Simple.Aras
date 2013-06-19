using System;
using System.Configuration;
using System.Security.Authentication;
using Aras.IOM;

namespace Simple.Aras
{
    public static class ArasInnovator
    {
        private const string DatabasePrefix = "database=";

        public static dynamic Open(IServerConnection connection)
        {
            return new ArasInnovatorAdaptor(IomFactory.CreateInnovator(connection));
        }

        public static dynamic OpenNamedConnection(string connectionStringName, bool loginImmediately = false)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionString == null) throw new ArgumentOutOfRangeException("connectionStringName");

            return OpenConnection(connectionString.ConnectionString, loginImmediately);
        }

        public static dynamic OpenConnection(string connection, bool loginImmediately = false)
        {
            return OpenConnection(new Uri(connection), loginImmediately);
        }

        public static dynamic OpenConnection(Uri connection, bool loginImmediately = false)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            var builder = new UriBuilder(connection);
            var pathParts = builder.Path.Split(';');
            if (pathParts.Length != 2 || false == pathParts[1].StartsWith(DatabasePrefix)) throw new ArgumentException("Expected /{Path};database={database}, received " + builder.Path + " instead.");

            var userName = builder.UserName;
            var password = builder.Password;
            var database = pathParts[1].Remove(0, DatabasePrefix.Length);

            builder.Path = pathParts[0];
            builder.UserName = builder.Password = String.Empty;
            
            var httpServerConnection = IomFactory.CreateHttpServerConnection(builder.Uri.ToString(), database, userName, password);
            if (loginImmediately)
            {
                LoginImmediately(httpServerConnection);
            }
            return new ArasInnovatorAdaptor(new Innovator(httpServerConnection));
        }

        private static void LoginImmediately(HttpServerConnection serverConnection)
        {
            var loginResult = serverConnection.Login();

            if (loginResult.isError())
            {
                throw new AuthenticationException(loginResult.getErrorString());
            }
        }
    }
}