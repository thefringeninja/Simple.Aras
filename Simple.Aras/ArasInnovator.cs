namespace Simple.Aras
{
    public static class ArasInnovator
    {
        public static dynamic Open(IArasHttpServerConnection connection)
        {
            return new ArasInnovatorAdaptor(connection);
        }
    }
}