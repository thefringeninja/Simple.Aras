namespace Simple.Aras
{
    public class ArasInnovatorAdaptor
    {
        private readonly IArasHttpServerConnection innovator;

        public ArasInnovatorAdaptor(IArasHttpServerConnection innovator)
        {
            this.innovator = innovator;
        }

        public ArasInnovatorMethodAdaptor Methods
        {
            get { return new ArasInnovatorMethodAdaptor(innovator); }
        }
    }
}