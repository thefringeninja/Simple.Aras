using System.Xml.Serialization;

namespace Simple.Aras.Tests
{
    [XmlRoot("TheRoot")]
    public class TestItem
    {
        public int Number { get; set; }
        public string Description { get; set; }
        [XmlElement("SomeOtherItem")]
        public OtherItem[] SomeOtherItems { get; set; }
    }

    public class OtherItem
    {
        public string Name { get; set; }
    }
}