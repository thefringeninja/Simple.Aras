using System;
using System.Dynamic;
using System.Linq;
using System.Xml.XPath;
using Aras.IOM;

namespace Simple.Aras
{
    public class ArasItem : DynamicObject
    {
        private readonly Item item;

        public ArasItem(string aml, Innovator innovator)
        {
            item = innovator.newItem();
            item.loadAML(aml);
        }

        public ArasItem(Item item)
        {
            this.item = item;
        }

        
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = Activator.CreateInstance(binder.ReturnType);

            var settableproperties =
                from node in item.node.CreateNavigator().Select("/Item/*[not(@is_null)]").OfType<XPathNavigator>()
                let property = binder.ReturnType.GetProperty(node.Name.Pascalize())
                where property != null
                select new {property, node};


            foreach (var settable in settableproperties)
            {
                var value = settable.property.PropertyType == typeof (Guid)
                    ? Guid.ParseExact(settable.node.Value, "N") 
                    : settable.node.ValueAs(settable.property.PropertyType);
                settable.property.SetValue(result, value, null);
            }

            return true;
        }
    }
}