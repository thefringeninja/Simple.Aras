using System;
using Aras.IOM;
using Moq;
using NUnit.Framework;
using Should.Fluent;

namespace Simple.Aras.Tests
{
    [TestFixture]
    public class casting_to_a_type_should : QuerySpecification<ArasItem, LifeCycleState>
    {
        private const string aml = @"
<Item type=""Life Cycle State"" typeId=""5EFB53D35BAE468B851CD388BEA46B30"" id=""5A600B427F1346E5A8B69D76989CFD05"">
    <id keyed_name=""Evaluating"" type=""Life Cycle State"">5A600B427F1346E5A8B69D76989CFD05</id>
    <keyed_name>Evaluating</keyed_name>
    <label is_null=""1"" />
    <name>Evaluating</name>
    <important_information>Cool Story Bro</important_information>
</Item>
";
        private IServerConnection connectionMock;
        protected override void Setup()
        {
            connectionMock = Mock.Of<IServerConnection>();
        }
        protected override ArasItem Given()
        {
            return new ArasItem(aml, new Innovator(connectionMock));
        }

        protected override Func<ArasItem, LifeCycleState> When()
        {
            return item => ((dynamic) item);
        }

        [Test]
        public  void parse_the_id_as_a_guid()
        {
            Result.Id.Should().Equal(Guid.Parse("5a600b42-7f13-46e5-a8b6-9d76989cfd05"));
        }
        [Test]
        public void treat_isnull_as_null()
        {
            Result.Label.Should().Be.Null();
        }
        [Test]
        public void load_properties()
        {
            Result.Name.Should().Equal("Evaluating");
        }
        [Test]
        public void convert_underscored_properties_to_camel_case()
        {
            Result.ImportantInformation.Should().Equal("Cool Story Bro");
        }
    

    }
}