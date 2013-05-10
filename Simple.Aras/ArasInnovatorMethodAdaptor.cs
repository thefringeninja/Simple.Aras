using System;
using System.Dynamic;
using Aras.IOM;

namespace Simple.Aras
{
    public class ArasInnovatorMethodAdaptor : DynamicObject
    {
        private readonly Innovator innovator;

        public ArasInnovatorMethodAdaptor(Innovator innovator)
        {
            this.innovator = innovator;
        }
        
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            string body = String.Empty;
            var argumentNames = binder.CallInfo.ArgumentNames;
            
            for (int i = 0; i < argumentNames.Count; i++)
            {
                body += String.Format("<{0}>{1}</{0}>", argumentNames[i], InnnovateValue(args[i]));
            }
            var resultOfApplication = innovator.applyMethod(binder.Name, body);
            if (resultOfApplication.isError())
            {
                throw new InvalidOperationException(resultOfApplication.getErrorString());
            }

            result = new ArasInnovatorItemAdaptor(resultOfApplication);

            return true;
        }

        private string InnnovateValue(object value)
        {
            if (value == null) return String.Empty;
            if (value is Guid)
            {
                return ((Guid) value).ToString("N").ToUpper();
            }
            return value.ToString();
        }
    }
}