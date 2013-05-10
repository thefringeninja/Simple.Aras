using System;
using System.Dynamic;
using System.IO;
using System.Xml.Serialization;
using Aras.IOM;

namespace Simple.Aras
{
    public class ArasInnovatorItemAdaptor : DynamicObject
    {
        private readonly Item item;

        public ArasInnovatorItemAdaptor(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (item.isError())
            {
                throw new ArgumentException("Item is an error.", "item");
            }

            this.item = item;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            if (binder.ReturnType == typeof (Item))
            {
                result = item;
                return true;
            };

            // pretty big assumption
            using (var itemResultReader = new StringReader(item.getResult()))
            {
                var serializer = new XmlSerializer(binder.ReturnType);

                result = serializer.Deserialize(itemResultReader);
                return true;
            }
        }
    }
}