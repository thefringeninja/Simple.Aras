using System;

namespace Simple.Aras.Tests
{
    public class LifeCycleState
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string ImportantInformation { get; set; }
        public string DoesNotExistInAras { get; set; }
        public double OddlyNamedProperty { get; set; }
        public int LengthOfName
        {
            get { return Name.Length; }
        }
    }
}