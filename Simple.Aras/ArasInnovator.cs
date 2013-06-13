﻿using System;
using System.Configuration;
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

        public static dynamic OpenNamedConnection(string connectionStringName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionString == null) throw new ArgumentOutOfRangeException("connectionStringName");

            return OpenConnection(connectionString.ConnectionString);
        }

        public static dynamic OpenConnection(string connection)
        {
            return OpenConnection(new Uri(connection));
        }

        public static dynamic OpenConnection(Uri connection)
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
            return new ArasInnovatorAdaptor(new Innovator(httpServerConnection));
        }
    }
}