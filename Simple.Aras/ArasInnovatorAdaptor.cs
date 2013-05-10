using Aras.IOM;

namespace Simple.Aras
{
    public class ArasInnovatorAdaptor
    {
        private readonly Innovator innovator;

        public ArasInnovatorAdaptor(Innovator innovator)
        {
            this.innovator = innovator;
        }

        public ArasInnovatorMethodAdaptor Methods
        {
            get { return new ArasInnovatorMethodAdaptor(innovator); }
        }
    }
}