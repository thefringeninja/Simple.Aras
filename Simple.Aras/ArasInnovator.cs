using Aras.IOM;

namespace Simple.Aras
{
    public static class ArasInnovator
    {
        public static dynamic Open(IServerConnection connection)
        {
            return new ArasInnovatorAdaptor(IomFactory.CreateInnovator(connection));
        }
    }
}