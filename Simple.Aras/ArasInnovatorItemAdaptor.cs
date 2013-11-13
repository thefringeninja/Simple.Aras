using System;
using System.Dynamic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Simple.Aras
{
    public class ArasInnovatorItemAdaptor : DynamicObject
    {
        private readonly Task<XElement> item;

        public ArasInnovatorItemAdaptor(Task<XElement> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            this.item = item;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (binder.Name != "GetAwaiter")
                return base.TryInvokeMember(binder, args, out result);

            result = item.GetAwaiter();

            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.ReturnType == typeof (XElement))
            {
                result = item.Result;
                return true;
            };

            if (typeof (Task).IsAssignableFrom(binder.ReturnType))
            {
                result = item;
                return true;
            }

            result = null;

            return false;
        }
    }
}