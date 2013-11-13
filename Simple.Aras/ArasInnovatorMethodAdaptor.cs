using System;
using System.Dynamic;
using System.Xml.Linq;


namespace Simple.Aras
{
    public class ArasInnovatorMethodAdaptor : DynamicObject
    {
        private readonly IArasHttpServerConnection innovator;

        public ArasInnovatorMethodAdaptor(IArasHttpServerConnection innovator)
        {
            this.innovator = innovator;
        }
        
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var item = new XElement(
                "Item", 
                new XAttribute("type", "Method"), 
                new XAttribute("action", binder.Name));

            var argumentNames = binder.CallInfo.ArgumentNames;

            for (int i = 0; i < argumentNames.Count; i++)
            {
                item.Add(new XElement(argumentNames[i], InnnovateValue(args[i])));
            }

            result = new ArasInnovatorItemAdaptor(innovator.ApplyAmlAsync(item));

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